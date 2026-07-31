using Application.Common.Models;
using Application.Common.Models.Auth;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Settings;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Features.Auth.Login;

public sealed class LoginHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuditService _auditService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IAuditService auditService,
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtSettings> jwtSettings,
        ILogger<LoginHandler> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _auditService = auditService;
        _httpContextAccessor = httpContextAccessor;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for email {Email}", request.Email);

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            _logger.LogWarning("Login failed: no user found with email {Email}", request.Email);
            return Result<AuthResponseDto>.Failure("Invalid credentials.", 401);
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Login failed: user {Email} is inactive", request.Email);
            return Result<AuthResponseDto>.Failure("User account is not active.", 403);
        }

        if (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow)
        {
            _logger.LogWarning("Login failed: user {Email} is locked out", request.Email);
            return Result<AuthResponseDto>.Failure("User account is locked out.", 403);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Login failed: invalid password for email {Email}", request.Email);
            return Result<AuthResponseDto>.Failure("Invalid credentials.", 401);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenService.GenerateAccessToken(roles, user.Id, user.UserName!, user.Email!);
        var refreshTokenValue = _tokenService.GenerateRefreshToken();
        var refreshTokenHash = _tokenService.HashToken(refreshTokenValue);

        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var existingTokens = await _refreshTokenRepository.GetByUserIdAsync(user.Id, cancellationToken);
        if (existingTokens is not null)
        {
            existingTokens.IsRevoked = true;
            existingTokens.RevokedDate = DateTime.UtcNow;
            existingTokens.RevokedByIp = ipAddress;
            _refreshTokenRepository.Update(existingTokens);
        }

        var refreshToken = new Domain.Entities.RefreshToken
        {
            Id = Guid.NewGuid(),
            TokenHash = refreshTokenHash,
            UserId = user.Id,
            ExpirationDate = DateTime.UtcNow.AddDays(
                _jwtSettings.RefreshTokenExpirationDays),
            CreatedDate = DateTime.UtcNow,
            CreatedByIp = ipAddress,
            IsRevoked = false
        };

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        await _auditService.LogAsync(
            user.Id,
            "UserLogin",
            "ApplicationUser",
            user.Id.ToString(),
            null,
            $"User {user.Email} logged in",
            ipAddress,
            cancellationToken);

        _logger.LogInformation("Successfully logged in user {Email}", request.Email);

        var response = new AuthResponseDto
        {
            AccessToken = accessToken,
            ExpirationTime = DateTime.UtcNow.AddMinutes(
                _jwtSettings.AccessTokenExpirationMinutes),
            RefreshToken = refreshTokenValue,
            UserId = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            Roles = roles
        };

        return Result<AuthResponseDto>.Success(response);
    }
}
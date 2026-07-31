using Application.Common.Models;
using Application.Common.Models.Auth;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Features.Auth.Register;

public sealed class RegisterHandler : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuditService _auditService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<RegisterHandler> _logger;

    public RegisterHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IAuditService auditService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<RegisterHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _auditService = auditService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registering new user with email {Email}", request.Email);

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            var errors = createResult.Errors.Select(e => e.Description);
            return Result<AuthResponseDto>.Failure(string.Join("; ", errors), 400);
        }

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>("User"));
        }

        await _userManager.AddToRoleAsync(user, "User");

        var roles = new[] { "User" };
        var accessToken = _tokenService.GenerateAccessToken(roles, user.Id, user.UserName!, user.Email!);
        var refreshTokenValue = _tokenService.GenerateRefreshToken();
        var refreshTokenHash = _tokenService.HashToken(refreshTokenValue);

        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            TokenHash = refreshTokenHash,
            UserId = user.Id,
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            CreatedDate = DateTime.UtcNow,
            CreatedByIp = ipAddress,
            IsRevoked = false
        };

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        await _auditService.LogAsync(
            user.Id,
            "UserRegistration",
            "ApplicationUser",
            user.Id.ToString(),
            null,
            $"User {user.Email} registered with User role",
            ipAddress,
            cancellationToken);

        _logger.LogInformation("Successfully registered user with Id {UserId}", user.Id);

        var response = new AuthResponseDto
        {
            AccessToken = accessToken,
            ExpirationTime = DateTime.UtcNow.AddMinutes(15),
            RefreshToken = refreshTokenValue,
            UserId = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            Roles = roles
        };

        return Result<AuthResponseDto>.Success(response);
    }
}
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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.Auth.RefreshTokenFeature;

public sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuditService _auditService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<RefreshTokenHandler> _logger;

    public RefreshTokenHandler(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IAuditService auditService,
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtSettings> jwtSettings,
        ILogger<RefreshTokenHandler> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _auditService = auditService;
        _httpContextAccessor = httpContextAccessor;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Refresh token attempt");

        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var principal = GetPrincipalFromExpiredToken(request.AccessToken);

        if (principal is null)
        {
            return Result<AuthResponseDto>.Failure("Invalid token.", 401);
        }

        var userIdString = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Result<AuthResponseDto>.Failure("Invalid token.", 401);
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null || !user.IsActive || user.SecurityStamp != principal.FindFirst("securityStamp")?.Value)
        {
            return Result<AuthResponseDto>.Failure("Invalid token.", 401);
        }

        var storedRefreshToken = await _refreshTokenRepository.GetByTokenHashAsync(
            _tokenService.HashToken(_httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"] ?? ""),
            cancellationToken);

        if (storedRefreshToken is null || storedRefreshToken.IsRevoked || storedRefreshToken.ExpirationDate <= DateTime.UtcNow)
        {
            return Result<AuthResponseDto>.Failure("Invalid refresh token.", 401);
        }

        if (storedRefreshToken.UserId != userId)
        {
            return Result<AuthResponseDto>.Failure("Invalid token.", 401);
        }

        storedRefreshToken.IsRevoked = true;
        storedRefreshToken.RevokedDate = DateTime.UtcNow;
        storedRefreshToken.RevokedByIp = ipAddress;
        _refreshTokenRepository.Update(storedRefreshToken);

        var newRefreshTokenValue = _tokenService.GenerateRefreshToken();
        var newRefreshTokenHash = _tokenService.HashToken(newRefreshTokenValue);

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            TokenHash = newRefreshTokenHash,
            UserId = userId,
            ExpirationDate = DateTime.UtcNow.AddDays(
                _jwtSettings.RefreshTokenExpirationDays),
            CreatedDate = DateTime.UtcNow,
            CreatedByIp = ipAddress,
            ReplacedByTokenHash = storedRefreshToken.TokenHash,
            IsRevoked = false
        };

        await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        var roles = await _userManager.GetRolesAsync(user);
        var newAccessToken = _tokenService.GenerateAccessToken(roles, user.Id, user.UserName!, user.Email!);

        await _auditService.LogAsync(
            userId,
            "TokenRefresh",
            "RefreshToken",
            newRefreshToken.Id.ToString(),
            $"Old token hash: {storedRefreshToken.TokenHash}",
            $"New token hash: {newRefreshTokenHash}",
            ipAddress,
            cancellationToken);

        _logger.LogInformation("Successfully refreshed token for user {UserId}", userId);

        var response = new AuthResponseDto
        {
            AccessToken = newAccessToken,
            ExpirationTime = DateTime.UtcNow.AddMinutes(
                _jwtSettings.AccessTokenExpirationMinutes),
            RefreshToken = newRefreshTokenValue,
            UserId = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            Roles = roles
        };

        return Result<AuthResponseDto>.Success(response);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            RequireSignedTokens = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            return principal;
        }
        catch
        {
            return null;
        }
    }
}
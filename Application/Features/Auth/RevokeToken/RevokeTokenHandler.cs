using Application.Common.Models;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Features.Auth.RevokeToken;

public sealed class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand, Result<bool>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;
    private readonly IAuditService _auditService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<RevokeTokenHandler> _logger;

    public RevokeTokenHandler(
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService,
        IAuditService auditService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<RevokeTokenHandler> logger)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
        _auditService = auditService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(
        RevokeTokenCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Revoke token attempt");

        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var tokenHash = _tokenService.HashToken(request.Token);

        var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash, cancellationToken);

        if (storedToken is null)
        {
            return Result<bool>.Failure("Invalid token.", 404);
        }

        if (storedToken.IsRevoked)
        {
            return Result<bool>.Failure("Token already revoked.", 400);
        }

        storedToken.IsRevoked = true;
        storedToken.RevokedDate = DateTime.UtcNow;
        storedToken.RevokedByIp = ipAddress;
        _refreshTokenRepository.Update(storedToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        await _auditService.LogAsync(
            storedToken.UserId,
            "TokenRevoke",
            "RefreshToken",
            storedToken.Id.ToString(),
            $"Token hash: {storedToken.TokenHash}",
            $"Revoked at {DateTime.UtcNow} by IP {ipAddress}",
            ipAddress,
            cancellationToken);

        _logger.LogInformation("Successfully revoked token for user {UserId}", storedToken.UserId);

        return Result<bool>.Success(true);
    }
}
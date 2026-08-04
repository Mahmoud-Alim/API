using System.Globalization;
using System.Security.Claims;
using System.Threading.RateLimiting;
using API.Constants;
using API.Json;
using API.Models;

namespace API.Services;

public sealed class RateLimitResponseFactory : IRateLimitResponseFactory
{
    private readonly ILogger<RateLimitResponseFactory> _logger;

    public RateLimitResponseFactory(ILogger<RateLimitResponseFactory> logger)
    {
        _logger = logger;
    }

    public async Task WriteRejectedResponseAsync(
        RateLimitRejectedContext context,
        CancellationToken cancellationToken)
    {
        var httpContext = context.HttpContext;

        httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        httpContext.Response.ContentType = ApiHeaders.JsonContentType;

        int? retryAfterSeconds = null;

        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            retryAfterSeconds = (int)retryAfter.TotalSeconds;

            httpContext.Response.Headers.RetryAfter =
                retryAfterSeconds.Value.ToString(CultureInfo.InvariantCulture);
        }

        var payload = new RateLimitExceededResponse
        {
            Success = false,
            Message = ApiErrors.RateLimitExceeded,
            RetryAfterSeconds = retryAfterSeconds
        };

        var userId = httpContext.User.FindFirstValue(ApiClaims.UserId);
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? ApiClaims.UnknownIp;
        var path = httpContext.Request.Path.Value ?? string.Empty;

        _logger.LogWarning(
            "Rate limit exceeded. Timestamp: {Timestamp}, UserId: {UserId}, IP: {Ip}, Path: {Path}, Policy: {Policy}",
            DateTimeOffset.UtcNow,
            userId ?? ApiClaims.UnknownIp,
            ip,
            path,
            context.PolicyName);

        await httpContext.Response.WriteAsJsonAsync(
            payload,
            JsonSerializerOptionsProvider.Default,
            cancellationToken);
    }
}

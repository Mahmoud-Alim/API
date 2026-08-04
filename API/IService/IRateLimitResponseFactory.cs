using System.Threading.RateLimiting;

namespace API.Services;

public interface IRateLimitResponseFactory
{
    Task WriteRejectedResponseAsync(
        RateLimitRejectedContext context,
        CancellationToken cancellationToken);
}

public sealed record RateLimitRejectedContext(
    HttpContext HttpContext,
    RateLimitLease Lease,
    string PolicyName);

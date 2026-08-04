using System.Security.Claims;
using API.Constants;

namespace API.Services;

public sealed class RemoteIpPartitionKeyProvider : IPartitionKeyProvider
{
    public string GetPartitionKey(HttpContext context)
    {
        var userId = context.User.FindFirstValue(ApiClaims.UserId);
        if (!string.IsNullOrWhiteSpace(userId))
        {
            return $"{RateLimiterConstants.AuthenticatedPartitionPrefix}{userId}";
        }

        var sub = context.User.FindFirstValue(ApiClaims.Sub);
        if (!string.IsNullOrWhiteSpace(sub))
        {
            return $"{RateLimiterConstants.AuthenticatedPartitionPrefix}{sub}";
        }

        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            ipAddress = ApiClaims.UnknownIp;
        }

        return $"{RateLimiterConstants.AnonymousPartitionPrefix}{ipAddress}";
    }
}

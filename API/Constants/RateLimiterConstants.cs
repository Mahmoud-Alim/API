using System.Threading.RateLimiting;

namespace API.Constants;

public static class RateLimiterConstants
{
    public const string PolicyName = "token";

    public const string AuthenticatedPartitionPrefix = "user:";

    public const string AnonymousPartitionPrefix = "ip:";

    public const QueueProcessingOrder QueueOrder = QueueProcessingOrder.OldestFirst;
}

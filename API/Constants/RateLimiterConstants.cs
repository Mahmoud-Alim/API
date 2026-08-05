using System.Threading.RateLimiting;

namespace API.Constants;

public static class RateLimiterConstants
{
    public const string PolicyName = "token";

    public const string AuthenticatedPartitionPrefix = "user:";

    public const string AnonymousPartitionPrefix = "ip:";

    public const QueueProcessingOrder QueueOrder = QueueProcessingOrder.OldestFirst;

    public const int DefaultGlobalPermitLimit = 100;

    public const int DefaultGlobalWindowSeconds = 60;

    public const int DefaultGlobalSegmentsPerWindow = 4;

    public const int DefaultTokenLimit = 5;

    public const int DefaultTokensPerPeriod = 1;

    public const int DefaultReplenishmentPeriodSeconds = 10;

    public const int DefaultQueueLimit = 0;
}

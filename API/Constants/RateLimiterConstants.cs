using System.Threading.RateLimiting;

namespace API.Constants;

public static class RateLimiterConstants
{
    public const string PolicyName = "token";

    public const int TokenLimit = 5;

    public const int TokensPerPeriod = 1;

    public const int ReplenishmentPeriodSeconds = 10;

    public const int QueueLimit = 0;

    public const QueueProcessingOrder QueueOrder = QueueProcessingOrder.OldestFirst;

    public static TimeSpan ReplenishmentPeriod => TimeSpan.FromSeconds(ReplenishmentPeriodSeconds);

    public const int GlobalPermitLimit = 100;

    public const int GlobalSegmentsPerWindow = 4;

    public static TimeSpan GlobalWindow => TimeSpan.FromMinutes(1);
}


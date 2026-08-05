using API.Constants;

namespace API.Configuration;

public sealed class RateLimiterSettings
{
    public const string SectionName = "RateLimiter";

    public int GlobalPermitLimit { get; init; } = RateLimiterConstants.DefaultGlobalPermitLimit;

    public int GlobalWindowSeconds { get; init; } = RateLimiterConstants.DefaultGlobalWindowSeconds;

    public int GlobalSegmentsPerWindow { get; init; } = RateLimiterConstants.DefaultGlobalSegmentsPerWindow;

    public int TokenLimit { get; init; } = RateLimiterConstants.DefaultTokenLimit;

    public int TokensPerPeriod { get; init; } = RateLimiterConstants.DefaultTokensPerPeriod;

    public int ReplenishmentPeriodSeconds { get; init; } = RateLimiterConstants.DefaultReplenishmentPeriodSeconds;

    public int QueueLimit { get; init; } = RateLimiterConstants.DefaultQueueLimit;
}

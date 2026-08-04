namespace API.Configuration;

public sealed class RateLimiterSettings
{
    public const string SectionName = "RateLimiter";

    public int GlobalPermitLimit { get; init; } = 100;

    public int GlobalWindowSeconds { get; init; } = 60;

    public int GlobalSegmentsPerWindow { get; init; } = 4;

    public int TokenLimit { get; init; } = 5;

    public int TokensPerPeriod { get; init; } = 1;

    public int ReplenishmentPeriodSeconds { get; init; } = 10;

    public int QueueLimit { get; init; } = 0;
}

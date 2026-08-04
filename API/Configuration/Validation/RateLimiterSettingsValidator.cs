using Microsoft.Extensions.Options;

namespace API.Configuration.Validation;

public sealed class RateLimiterSettingsValidator : IValidateOptions<RateLimiterSettings>
{
    public ValidateOptionsResult Validate(string? name, RateLimiterSettings options)
    {
        var failures = new List<string>();

        if (options.GlobalPermitLimit <= 0)
        {
            failures.Add("RateLimiter:GlobalPermitLimit must be greater than zero.");
        }

        if (options.GlobalWindowSeconds <= 0)
        {
            failures.Add("RateLimiter:GlobalWindowSeconds must be greater than zero.");
        }

        if (options.GlobalSegmentsPerWindow <= 0)
        {
            failures.Add("RateLimiter:GlobalSegmentsPerWindow must be greater than zero.");
        }

        if (options.TokenLimit <= 0)
        {
            failures.Add("RateLimiter:TokenLimit must be greater than zero.");
        }

        if (options.TokensPerPeriod <= 0)
        {
            failures.Add("RateLimiter:TokensPerPeriod must be greater than zero.");
        }

        if (options.ReplenishmentPeriodSeconds <= 0)
        {
            failures.Add("RateLimiter:ReplenishmentPeriodSeconds must be greater than zero.");
        }

        if (options.QueueLimit < 0)
        {
            failures.Add("RateLimiter:QueueLimit must be greater than or equal to zero.");
        }

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }
}

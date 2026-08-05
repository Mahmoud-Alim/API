using Microsoft.Extensions.Options;

namespace API.Configuration.Validation;

public sealed class RateLimiterSettingsValidator : IValidateOptions<RateLimiterSettings>
{
    public ValidateOptionsResult Validate(string? name, RateLimiterSettings options)
    {
        var failures = new List<string>();

if (options.GlobalPermitLimit <= 0)
        {
            failures.Add(ConfigurationMessages.RateLimiterGlobalPermitLimit);
        }

        if (options.GlobalWindowSeconds <= 0)
        {
            failures.Add(ConfigurationMessages.RateLimiterGlobalWindowSeconds);
        }

        if (options.GlobalSegmentsPerWindow <= 0)
        {
            failures.Add(ConfigurationMessages.RateLimiterGlobalSegmentsPerWindow);
        }

        if (options.TokenLimit <= 0)
        {
            failures.Add(ConfigurationMessages.RateLimiterTokenLimit);
        }

        if (options.TokensPerPeriod <= 0)
        {
            failures.Add(ConfigurationMessages.RateLimiterTokensPerPeriod);
        }

        if (options.ReplenishmentPeriodSeconds <= 0)
        {
            failures.Add(ConfigurationMessages.RateLimiterReplenishmentPeriodSeconds);
        }

        if (options.QueueLimit < 0)
        {
            failures.Add(ConfigurationMessages.RateLimiterQueueLimit);
        }

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }
}

using System.Threading.RateLimiting;
using API.Configuration;
using API.Constants;
using API.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace API.Extensions;

public static class RateLimiterExtensions
{
    public static IServiceCollection AddRateLimiterServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IPartitionKeyProvider, RemoteIpPartitionKeyProvider>();
        services.AddSingleton<IRateLimitResponseFactory, RateLimitResponseFactory>();

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (context, cancellationToken) =>
            {
                var responseFactory = context.HttpContext
                    .RequestServices
                    .GetRequiredService<IRateLimitResponseFactory>();

                var rejectedContext = new RateLimitRejectedContext(
                    context.HttpContext,
                    context.Lease,
                    RateLimiterConstants.PolicyName);

                await responseFactory.WriteRejectedResponseAsync(rejectedContext, cancellationToken);
            };
        });

        services.AddOptions<RateLimiterOptions>()
            .PostConfigure<IOptions<RateLimiterSettings>>((options, settingsOptions) =>
            {
                var settings = settingsOptions.Value;

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                    context =>
                    {
                        var partitionKeyProvider = context
                            .RequestServices
                            .GetRequiredService<IPartitionKeyProvider>();

                        return RateLimitPartition.GetSlidingWindowLimiter(
                            partitionKey: partitionKeyProvider.GetPartitionKey(context),
                            factory: _ => new SlidingWindowRateLimiterOptions
                            {
                                PermitLimit = settings.GlobalPermitLimit,
                                Window = TimeSpan.FromSeconds(settings.GlobalWindowSeconds),
                                SegmentsPerWindow = settings.GlobalSegmentsPerWindow,
                                AutoReplenishment = true
                            });
                    });

                options.AddPolicy(RateLimiterConstants.PolicyName, context =>
                {
                    var partitionKeyProvider = context
                        .RequestServices
                        .GetRequiredService<IPartitionKeyProvider>();

                    return RateLimitPartition.GetTokenBucketLimiter(
                        partitionKey: partitionKeyProvider.GetPartitionKey(context),
                        factory: _ => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = settings.TokenLimit,
                            TokensPerPeriod = settings.TokensPerPeriod,
                            ReplenishmentPeriod = TimeSpan.FromSeconds(settings.ReplenishmentPeriodSeconds),
                            AutoReplenishment = true,
                            QueueLimit = settings.QueueLimit,
                            QueueProcessingOrder = RateLimiterConstants.QueueOrder
                        });
                });
            });

        return services;
    }
}

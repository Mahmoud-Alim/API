using API.Configuration;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace API.Extensions;

public static class CorsExtensions
{
    public const string CorsPolicyName = "CorsPolicy";

    public static IServiceCollection AddCorsServices(
        this IServiceCollection services)
    {
        services.AddOptions<CorsOptions>()
            .Configure<IOptions<CorsSettings>>((options, settingsOptions) =>
            {
                var corsSettings = settingsOptions.Value;

                options.AddPolicy(CorsPolicyName, policy =>
                {
                    policy
                        .WithOrigins(corsSettings.AllowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader();

                    if (corsSettings.AllowCredentials)
                    {
                        policy.AllowCredentials();
                    }
                });
            });

        services.AddCors();

        return services;
    }
}

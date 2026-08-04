using API.Configuration;
using API.Configuration.Validation;
using API.Extensions;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();

        services.AddPresentationOpenApi();

        services.AddPresentationAuthentication();

        services.AddPresentationAuthorization();

        services.ConfigureOptions<CorsSettingsValidator>();
        services.ConfigureOptions<RateLimiterSettingsValidator>();

        services.AddOptions<CorsSettings>()
            .Bind(configuration.GetSection(CorsSettings.SectionName))
            .ValidateOnStart();

        services.AddOptions<RateLimiterSettings>()
            .Bind(configuration.GetSection(RateLimiterSettings.SectionName))
            .ValidateOnStart();

        services.AddOptions<ForwardedHeadersSettings>()
            .Bind(configuration.GetSection(ForwardedHeadersSettings.SectionName))
            .ValidateOnStart();

        services.AddForwardedHeadersServices();

        services.AddCorsServices();

        services.AddRateLimiterServices();

        return services;
    }

    public static WebApplication UsePresentationServices(this WebApplication app)
    {
        app.UsePresentationMiddleware();

        return app;
    }

    public static WebApplication MapPresentationEndpoints(this WebApplication app)
    {
        app.MapEndpoints();

        return app;
    }
}

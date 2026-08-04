namespace API.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddPresentationOpenApi(
        this IServiceCollection services)
    {
        services.AddOpenApi();

        return services;
    }

    public static WebApplication MapPresentationOpenApi(
        this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi().AllowAnonymous();
        }

        return app;
    }
}

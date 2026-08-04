namespace API.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapPresentationOpenApi();

        app.MapControllers();

        return app;
    }
}

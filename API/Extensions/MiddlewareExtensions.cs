using API.Constants;
using API.Middleware;
using Serilog;

namespace API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UsePresentationMiddleware(
        this IApplicationBuilder app)
    {
        app.UseForwardedHeaders();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseSerilogRequestLogging();

        app.UseRouting();

        app.UseCors(CorsExtensions.CorsPolicyName);

        app.UseAuthentication();

        app.UseRateLimiter();

        app.UseAuthorization();

        return app;
    }
}

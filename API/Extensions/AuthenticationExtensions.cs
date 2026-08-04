namespace API.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddPresentationAuthentication(
        this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        return services;
    }
}

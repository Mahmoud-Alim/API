using Microsoft.AspNetCore.Authorization;

namespace API.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddPresentationAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }
}

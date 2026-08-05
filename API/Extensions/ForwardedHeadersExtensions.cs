using System.Net;
using API.Configuration;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;

namespace API.Extensions;

public static class ForwardedHeadersExtensions
{
    public static IServiceCollection AddForwardedHeadersServices(
        this IServiceCollection services)
    {
        services.AddOptions<ForwardedHeadersOptions>()
            .Configure<IOptions<ForwardedHeadersSettings>>((options, settingsOptions) =>
            {
                var settings = settingsOptions.Value;

                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                options.RequireHeaderSymmetry = false;

                if (!settings.Enabled)
                {
                    return;
                }

                options.KnownProxies.Add(IPAddress.Loopback);
                options.KnownProxies.Add(IPAddress.IPv6Loopback);

                if (settings.ForwardLimit is > 0)
                {
                    options.ForwardLimit = settings.ForwardLimit;
                }
            });

        return services;
    }
}
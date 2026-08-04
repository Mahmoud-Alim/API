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

                if (settings.KnownNetworks.Length == 0 &&
                    settings.KnownProxies.Length == 0)
                {
                    throw new InvalidOperationException(
                        "ForwardedHeaders:KnownNetworks or ForwardedHeaders:KnownProxies " +
                        "must be configured when the application runs behind a reverse proxy. " +
                        "Refusing to trust all networks/proxies.");
                }

                foreach (var knownNetwork in settings.KnownNetworks)
                {
                    var (prefix, prefixLength) = ParseNetwork(knownNetwork);
                    var network = new System.Net.IPNetwork(prefix, prefixLength);
                    options.KnownIPNetworks.Add(network);
                }

                foreach (var knownProxy in settings.KnownProxies)
                {
                    if (IPAddress.TryParse(knownProxy, out var address))
                    {
                        options.KnownProxies.Add(address);
                        continue;
                    }

                    throw new InvalidOperationException(
                        $"ForwardedHeaders:KnownProxies contains an invalid IP address: '{knownProxy}'.");
                }

                if (settings.ForwardLimit is > 0)
                {
                    options.ForwardLimit = settings.ForwardLimit;
                }
            });

        return services;
    }

    private static (IPAddress Prefix, int PrefixLength) ParseNetwork(string cidr)
    {
        var parts = cidr.Split('/', 2);
        if (parts.Length != 2 ||
            !IPAddress.TryParse(parts[0], out var prefix) ||
            !int.TryParse(parts[1], out var prefixLength))
        {
            throw new InvalidOperationException(
                $"ForwardedHeaders:KnownNetworks contains an invalid CIDR: '{cidr}'. " +
                "Expected format: '192.168.0.0/16' or '10.0.0.0/8'.");
        }

        return (prefix, prefixLength);
    }
}

namespace API.Configuration.Validation;

public static class ConfigurationMessages
{
    public const string CorsAllowedOriginsRequired =
        "Cors:AllowedOrigins must contain at least one origin.";

    public const string CorsAllowedOriginsWildcard =
        "Cors:AllowedOrigins must not contain the wildcard '*' origin " +
        "because AllowCredentials cannot be used with AllowAnyOrigin.";

    public const string CorsAllowedOriginsWhitespaceFormat =
        "Cors:AllowedOrigins[{0}] is empty or contains only whitespace. " +
        "Each origin must be trimmed and non-empty.";

    public const string CorsAllowedOriginsDuplicateFormat =
        "Cors:AllowedOrigins contains a duplicate origin: '{0}'. " +
        "Each origin must appear only once.";

    public const string CorsAllowedOriginsInvalidFormat =
        "Cors:AllowedOrigins contains an invalid origin: '{0}'. " +
        "An origin must be an absolute URI with scheme and host only, " +
        "and must not contain a path, query string, or fragment.";

    public const string RateLimiterGlobalPermitLimit =
        "RateLimiter:GlobalPermitLimit must be greater than zero.";

    public const string RateLimiterGlobalWindowSeconds =
        "RateLimiter:GlobalWindowSeconds must be greater than zero.";

    public const string RateLimiterGlobalSegmentsPerWindow =
        "RateLimiter:GlobalSegmentsPerWindow must be greater than zero.";

    public const string RateLimiterTokenLimit =
        "RateLimiter:TokenLimit must be greater than zero.";

    public const string RateLimiterTokensPerPeriod =
        "RateLimiter:TokensPerPeriod must be greater than zero.";

    public const string RateLimiterReplenishmentPeriodSeconds =
        "RateLimiter:ReplenishmentPeriodSeconds must be greater than zero.";

    public const string RateLimiterQueueLimit =
        "RateLimiter:QueueLimit must be greater than or equal to zero.";

    public const string ForwardedHeadersNetworksRequired =
        "ForwardedHeaders:KnownNetworks or ForwardedHeaders:KnownProxies " +
        "must be configured when the application runs behind a reverse proxy. " +
        "Refusing to trust all networks/proxies.";

    public const string ForwardedHeadersInvalidProxyFormat =
        "ForwardedHeaders:KnownProxies contains an invalid IP address: '{0}'.";

public const string ForwardedHeadersInvalidCidrFormat =
        "ForwardedHeaders:KnownNetworks contains an invalid CIDR: '{0}'. " +
        "Expected format: '192.168.0.0/16' or '10.0.0.0/8'.";
}

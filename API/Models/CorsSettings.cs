namespace API.Models;

/// <summary>
/// Strongly-typed settings bound to the "Cors" section of appsettings.json.
/// </summary>
public sealed class CorsSettings
{
    /// <summary>The configuration section this settings object is bound to.</summary>
    public const string SectionName = "Cors";

    /// <summary>Origins allowed to call the API (e.g. http://localhost:3000).</summary>
    public string[] AllowedOrigins { get; init; } = [];

    /// <summary>
    /// Whether the CORS policy may send credentials (cookies, Authorization headers).
    /// Only applicable when specific origins are configured and none is the wildcard "*".
    /// </summary>
    public bool AllowCredentials { get; init; }
}


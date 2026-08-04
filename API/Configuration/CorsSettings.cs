namespace API.Configuration;

public sealed class CorsSettings
{
    public const string SectionName = "Cors";

    public string[] AllowedOrigins { get; init; } = [];

    public bool AllowCredentials { get; init; }
}

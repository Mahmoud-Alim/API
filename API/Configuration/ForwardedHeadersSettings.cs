namespace API.Configuration;

public sealed class ForwardedHeadersSettings
{
    public const string SectionName = "ForwardedHeaders";

    public bool Enabled { get; init; } = true;

    public string[] KnownNetworks { get; init; } = [];

    public string[] KnownProxies { get; init; } = [];

    public int? ForwardLimit { get; init; }
}

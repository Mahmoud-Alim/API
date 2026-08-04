using Microsoft.Extensions.Options;

namespace API.Configuration.Validation;

public sealed class CorsSettingsValidator : IValidateOptions<CorsSettings>
{
    public ValidateOptionsResult Validate(string? name, CorsSettings options)
    {
        var failures = new List<string>();

        ValidateNotEmpty(options.AllowedOrigins, failures);

        if (failures.Count == 0)
        {
            ValidateWildcard(options.AllowedOrigins, failures);
            ValidateWhitespace(options.AllowedOrigins, failures);
            ValidateDuplicates(options.AllowedOrigins, failures);
            ValidateOriginsAreRealOrigins(options.AllowedOrigins, failures);
        }

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }

    private static void ValidateNotEmpty(string[]? origins, List<string> failures)
    {
        if (origins is null || origins.Length == 0)
        {
            failures.Add("Cors:AllowedOrigins must contain at least one origin.");
        }
    }

    private static void ValidateWildcard(string[] origins, List<string> failures)
    {
        if (origins.Contains("*", StringComparer.Ordinal))
        {
            failures.Add(
                "Cors:AllowedOrigins must not contain the wildcard '*' origin " +
                "because AllowCredentials cannot be used with AllowAnyOrigin.");
        }
    }

    private static void ValidateWhitespace(string[] origins, List<string> failures)
    {
        var emptyOrigins = origins
            .Select((origin, index) => (Origin: origin, Index: index))
            .Where(item => string.IsNullOrWhiteSpace(item.Origin.Trim()))
            .Select(item => item.Index);

        foreach (var index in emptyOrigins)
        {
            failures.Add(
                $"Cors:AllowedOrigins[{index}] is empty or contains only whitespace. " +
                "Each origin must be trimmed and non-empty.");
        }
    }

    private static void ValidateDuplicates(string[] origins, List<string> failures)
    {
        var duplicates = origins
            .Select(origin => origin.Trim())
            .Where(origin => origin.Length > 0)
            .GroupBy(origin => origin, StringComparer.OrdinalIgnoreCase)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        foreach (var duplicate in duplicates)
        {
            failures.Add(
                $"Cors:AllowedOrigins contains a duplicate origin: '{duplicate}'. " +
                "Each origin must appear only once.");
        }
    }

    private static void ValidateOriginsAreRealOrigins(string[] origins, List<string> failures)
    {
        foreach (var origin in origins)
        {
            var trimmed = origin.Trim();

            if (trimmed.Length == 0)
            {
                continue;
            }

            if (!IsValidOrigin(trimmed))
            {
                failures.Add(
                    $"Cors:AllowedOrigins contains an invalid origin: '{origin}'. " +
                    "An origin must be an absolute URI with scheme and host only, " +
                    "and must not contain a path, query string, or fragment.");
            }
        }
    }

    private static bool IsValidOrigin(string origin)
    {
        if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
        {
            return false;
        }

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        {
            return false;
        }

        return !HasPathQueryOrFragment(uri);
    }

    private static bool HasPathQueryOrFragment(Uri uri)
    {
        var hasPath = !string.IsNullOrEmpty(uri.AbsolutePath)
            && uri.AbsolutePath != "/";

        var hasQuery = !string.IsNullOrEmpty(uri.Query);
        var hasFragment = !string.IsNullOrEmpty(uri.Fragment);

        return hasPath || hasQuery || hasFragment;
    }
}

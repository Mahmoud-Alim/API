namespace API.Models;

/// <summary>
/// Standard response body returned when a request is rejected by the rate limiter (HTTP 429).
/// Serialized with camelCase so the JSON is: {"success": false, "message": "..."}.
/// </summary>
public sealed class RateLimitExceededResponse
{
    /// <summary>Always <c>false</c> for a rejected request.</summary>
    public bool Success { get; init; }

    /// <summary>User-friendly explanation of why the request was rejected.</summary>
    public string Message { get; init; } = string.Empty;
}


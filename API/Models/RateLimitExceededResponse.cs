namespace API.Models;

public sealed class RateLimitExceededResponse
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;

    public int? RetryAfterSeconds { get; init; }
}


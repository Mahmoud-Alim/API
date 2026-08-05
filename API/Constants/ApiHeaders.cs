using Domain.Constants;

namespace API.Constants;

public static class ApiHeaders
{
    public const string RefreshToken = CookieNames.RefreshToken;

    public const string RetryAfter = "Retry-After";

    public const string JsonContentType = "application/json";

    public const string ProblemJsonContentType = "application/problem+json";
}

namespace API.Constants;

public static class ApiErrors
{
    public const string RateLimitExceeded =
        "Rate limit exceeded. Please try again later.";

    public const string CorsConfigurationMissing =
        "CORS is not configured. 'Cors:AllowedOrigins' must contain at least one origin.";

    public const string UnexpectedError =
        "An unexpected error occurred. Please try again later.";

    public const string ValidationFailed =
        "One or more validation errors occurred.";

    public const string Unauthorized =
        "You are not authorized to access this resource.";

    public const string Forbidden =
        "You do not have permission to access this resource.";

    public const string NoRefreshToken =
        "No refresh token found in cookies.";

public const string AnErrorOccurred =
        "An error occurred.";

    public const string RateLimitEndpointReached =
        "Rate limited endpoint reached successfully.";
}

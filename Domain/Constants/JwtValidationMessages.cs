namespace Domain.Constants;

public static class JwtValidationMessages
{
    public const string SecretKeyNotConfigured =
        "JWT SecretKey is not configured. Set 'Jwt__SecretKey' in the .env file " +
        "(or 'Jwt:SecretKey' in appsettings.json) to a value of at least 32 characters (256 bits).";

    public const string SecretKeyTooShort =
        "JWT SecretKey is too short. HS256 requires a key of at least 32 characters (256 bits).";

    public const string IssuerNotConfigured =
        "JWT Issuer is not configured. Set 'Jwt__Issuer' in the .env file " +
        "(or 'Jwt:Issuer' in appsettings.json).";

    public const string AudienceNotConfigured =
        "JWT Audience is not configured. Set 'Jwt__Audience' in the .env file " +
        "(or 'Jwt:Audience' in appsettings.json).";

    public const string AccessTokenExpirationInvalid =
        "JWT AccessTokenExpirationMinutes must be greater than zero. " +
        "Set 'Jwt__AccessTokenExpirationMinutes' in the .env file.";

    public const string RefreshTokenExpirationInvalid =
        "JWT RefreshTokenExpirationDays must be greater than zero. " +
        "Set 'Jwt__RefreshTokenExpirationDays' in the .env file.";
}

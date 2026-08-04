namespace Infrastructure.Configuration;

public sealed class IdentitySettings
{
    public const string SectionName = "Identity";

    public bool PasswordRequireDigit { get; init; } = true;

    public bool PasswordRequireLowercase { get; init; } = true;

    public bool PasswordRequireUppercase { get; init; } = true;

    public bool PasswordRequireNonAlphanumeric { get; init; } = true;

    public int PasswordRequiredLength { get; init; } = 8;

    public bool UserRequireUniqueEmail { get; init; } = true;

    public int LockoutDefaultLockoutTimeSpanMinutes { get; init; } = 15;

    public int LockoutMaxFailedAccessAttempts { get; init; } = 5;

    public bool LockoutAllowedForNewUsers { get; init; } = true;

    public string AllowedUserNameCharacters { get; init; } =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
}

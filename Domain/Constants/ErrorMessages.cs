namespace Domain.Constants;

public static class ErrorMessages
{
    public const string InvalidCredentials = "Invalid credentials.";

    public const string UserInactive = "User account is not active.";

    public const string UserLockedOut = "User account is locked out.";

    public const string InvalidToken = "Invalid token.";

    public const string InvalidRefreshToken = "Invalid refresh token.";

    public const string TokenAlreadyRevoked = "Token already revoked.";

    public const string UserNotFound = "User not found.";

    public const string CannotModifyBoss = "Cannot modify Boss users.";

    public const string UserAlreadyAdmin = "User is already an Admin.";

    public const string AdminRoleDoesNotExist = "Admin role does not exist.";

    public const string CannotRemoveBossRole = "Cannot remove the Boss role.";

    public const string UnknownIp = "unknown";
}

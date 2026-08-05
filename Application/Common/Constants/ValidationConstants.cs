namespace Application.Common.Constants;

public static class ValidationConstants
{
    // Lengths
    public const int NameMinLength = 3;
    public const int NameMaxLength = 100;
    public const int EmailMaxLength = 255;
    public const int PasswordMinLength = 8;
    public const int PasswordMaxLength = 128;
    public const int RoleNameMinLength = 3;
    public const int RoleNameMaxLength = 50;
    public const int GenderMinLength = 4;
    public const int GenderMaxLength = 20;
    public const int SearchTermMaxLength = 100;
    public const int PageSizeMaxValue = 100;

    // Allowed sort fields
    public static readonly string[] AllowedSortFields =
    {
        "UserId", "FirstName", "LastName", "Email", "Gender", "Active"
    };

    // Allowed gender values
    public static readonly string[] AllowedGenders =
    {
        "Male", "Female"
    };

    // Error messages
    public const string UserIdGreaterThanZeroMessage = "User Id must be greater than zero.";
    public const string FirstNameRequiredMessage = "First name is required.";
    public const string FirstNameMinLengthMessage = "First name must be at least 3 characters.";
    public const string FirstNameMaxLengthMessage = "First name must not exceed 100 characters.";
    public const string FirstNameLengthRangeMessage = "First name must be between 3 and 100 characters.";
    public const string LastNameRequiredMessage = "Last name is required.";
    public const string LastNameMinLengthMessage = "Last name must be at least 3 characters.";
    public const string LastNameMaxLengthMessage = "Last name must not exceed 100 characters.";
    public const string LastNameLengthRangeMessage = "Last name must be between 3 and 100 characters.";
    public const string EmailRequiredMessage = "Email is required.";
public const string EmailValidMessage = "A valid email address is required.";
    public const string EmailMaxLengthMessage = "Email must not exceed 255 characters.";
    public const string EmailMaxLength200Message = "Email must not exceed 200 characters.";
    public const string PasswordRequiredMessage = "Password is required.";
    public const string PasswordMinLengthMessage = "Password must be at least 8 characters.";
    public const string PasswordMaxLengthMessage = "Password must not exceed 128 characters.";
    public const string RoleNameRequiredMessage = "Role name is required.";
    public const string RoleNameMinLengthMessage = "Role name must be at least 3 characters.";
    public const string RoleNameMaxLengthMessage = "Role name must not exceed 50 characters.";
    public const string GenderRequiredMessage = "Gender is required.";
    public const string GenderLengthRangeMessage = "Gender must be between 4 and 20 characters.";
    public const string GenderMaxLengthMessage = "Gender must not exceed 20 characters.";
    public const string GenderAllowedMessage = "Gender must be either 'Male' or 'Female'.";
    public const string PageNumberGreaterThanZeroMessage = "Page number must be greater than zero.";
    public const string PageSizeGreaterThanZeroMessage = "Page size must be greater than zero.";
    public const string PageSizeMaxMessage = "Page size must not exceed 100.";
    public const string SortByAllowedMessage = "SortBy must be one of: UserId, FirstName, LastName, Email, Gender, Active.";
    public const string SearchTermMaxLengthMessage = "Search term must not exceed 100 characters.";
    public const string AccessTokenRequiredMessage = "Access token is required.";
    public const string TokenRequiredMessage = "Token is required.";
    public const string UserIdRequiredMessage = "User ID is required.";
}

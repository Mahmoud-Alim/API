namespace Application.Constants;

public static class ValidationRules
{
    public const int FirstNameMinLength = 3;
    public const int FirstNameMaxLength = 100;

    public const int LastNameMinLength = 3;
    public const int LastNameMaxLength = 100;

    public const int EmailMaxLength = 200;

    public const int PasswordMinLength = 8;
    public const int PasswordMaxLength = 128;

    public const int GenderMinLength = 4;
    public const int GenderMaxLength = 20;

    public const int RoleNameMinLength = 3;
    public const int RoleNameMaxLength = 50;

    public const int SearchTermMaxLength = 100;

    public const int PageSizeMax = 100;
}

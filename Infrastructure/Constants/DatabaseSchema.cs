namespace Infrastructure.Constants;

public static class DatabaseSchema
{
    public const string UsersTable = "Users";
    public const string UserJobInfosTable = "UserJobInfos";
    public const string UserSalariesTable = "UserSalaries";
    public const string RefreshTokensTable = "RefreshTokens";
    public const string AuditLogsTable = "AuditLogs";
    public const string AspNetUsersTable = "AspNetUsers";

    public const string Decimal18_2 = "decimal(18,2)";

    public const int UserFirstNameMaxLength = 100;
    public const int UserLastNameMaxLength = 100;
    public const int UserEmailMaxLength = 255;
    public const int UserGenderMaxLength = 20;

    public const int JobTitleMaxLength = 100;
    public const int DepartmentMaxLength = 100;

    public const int TokenHashMaxLength = 64;
    public const int IpAddressMaxLength = 45;

    public const int AuditActionMaxLength = 100;
    public const int AuditEntityNameMaxLength = 100;
    public const int AuditEntityIdMaxLength = 100;
    public const int AuditValuesMaxLength = 2000;
}

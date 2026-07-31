namespace API.Constants;

public static class RouteConstants
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = $"{Root}/{Version}";

    public static class Auth
    {
        public const string Base = $"{RouteConstants.Base}/auth";
        public const string Register = "register";
        public const string Login = "login";
        public const string RefreshToken = "refresh-token";
        public const string Logout = "Logout";
    }

    public static class Users
    {
        public const string Base = $"{RouteConstants.Base}/users";
        public const string GetActive = "active";
        public const string GetById = "{id}";
        public const string GetJobInfo = "{id}/job-info";
        public const string GetSalary = "{id}/salary";
        public const string Update = "{id}";
        public const string Remove = "{id}";
        public const string UserExists = "{id}/exists";
    }

    public static class Roles
    {
        public const string Base = $"{RouteConstants.Base}/roles";
        public const string PromoteAdmin = "promote-admin/{userId}";
        public const string Add = "add";
        public const string Remove = "remove";
        public const string GetUserRoles = "{userId}";
    }

    public static class RateLimitDemo
    {
        public const string Base = $"{RouteConstants.Base}/rate-limit";
    }
}
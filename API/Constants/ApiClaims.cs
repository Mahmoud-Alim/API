using System.Security.Claims;

namespace API.Constants;

public static class ApiClaims
{
    public const string UserId = ClaimTypes.NameIdentifier;

    public const string Sub = "sub";

    public const string Role = ClaimTypes.Role;

    public const string Email = ClaimTypes.Email;

    public const string Name = ClaimTypes.Name;

    public const string SecurityStamp = "securityStamp";

    public const string UnknownIp = "unknown";
}

namespace Application.Common.Models.Auth;

public sealed class UserRoleDto
{
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public IEnumerable<string> Roles { get; init; } = Enumerable.Empty<string>();
}
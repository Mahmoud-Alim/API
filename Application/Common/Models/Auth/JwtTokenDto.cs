namespace Application.Common.Models.Auth;

public sealed class JwtTokenDto
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime ExpirationTime { get; init; }
}
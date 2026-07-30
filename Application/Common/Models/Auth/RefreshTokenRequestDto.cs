namespace Application.Common.Models.Auth;

public sealed class RefreshTokenRequestDto
{
    public string AccessToken { get; init; } = string.Empty;
}
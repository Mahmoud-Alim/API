namespace Application.Common.Models.Auth;

public sealed class RevokeTokenRequestDto
{
    public string Token { get; init; } = string.Empty;
}
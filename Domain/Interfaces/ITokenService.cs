namespace Domain.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<string> roles, Guid userId, string username, string email);
    string GenerateRefreshToken();
    string HashToken(string token);
    bool VerifyTokenHash(string token, string hash);
}
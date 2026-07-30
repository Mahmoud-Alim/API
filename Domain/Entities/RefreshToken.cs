namespace Domain.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? RevokedDate { get; set; }
    public string CreatedByIp { get; set; } = string.Empty;
    public string? RevokedByIp { get; set; }
    public string? ReplacedByTokenHash { get; set; }
    public bool IsRevoked { get; set; }
    public ApplicationUser User { get; set; } = null!;
}
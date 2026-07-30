namespace Domain.Entities;

public sealed class AuditLog
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public DateTime CreatedDate { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
}
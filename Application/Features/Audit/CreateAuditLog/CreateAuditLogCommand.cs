using Application.Common.Models;
using MediatR;

namespace Application.Features.Audit.CreateAuditLog;

public sealed record CreateAuditLogCommand : IRequest<Result<bool>>
{
    public Guid UserId { get; init; }
    public string Action { get; init; } = string.Empty;
    public string EntityName { get; init; } = string.Empty;
    public string? EntityId { get; init; }
    public string? OldValues { get; init; }
    public string? NewValues { get; init; }
    public string IpAddress { get; init; } = string.Empty;
}
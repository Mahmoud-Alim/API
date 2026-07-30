namespace Domain.Interfaces;

public interface IAuditService
{
    Task LogAsync(Guid userId,
                  string action,
                  string entityName,
                  string? entityId,
                  string? oldValues,
                  string? newValues,
                  string ipAddress,
                  CancellationToken cancellationToken = default);
}
using Application.Common.Models;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Audit.CreateAuditLog;

public sealed class CreateAuditLogHandler : IRequestHandler<CreateAuditLogCommand, Result<bool>>
{
    private readonly IAuditService _auditService;
    private readonly ILogger<CreateAuditLogHandler> _logger;

    public CreateAuditLogHandler(
        IAuditService auditService,
        ILogger<CreateAuditLogHandler> logger)
    {
        _auditService = auditService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(
        CreateAuditLogCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating audit log: {Action} on {EntityName} by user {UserId}",
            request.Action,
            request.EntityName,
            request.UserId);

        await _auditService.LogAsync(
            request.UserId,
            request.Action,
            request.EntityName,
            request.EntityId,
            request.OldValues,
            request.NewValues,
            request.IpAddress,
            cancellationToken);

        return Result<bool>.Success(true);
    }
}
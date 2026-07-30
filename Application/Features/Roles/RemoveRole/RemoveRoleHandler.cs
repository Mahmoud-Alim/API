using Application.Common.Models;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Features.Roles.RemoveRole;

public sealed class RemoveRoleHandler : IRequestHandler<RemoveRoleCommand, Result<bool>>
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IAuditService _auditService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<RemoveRoleHandler> _logger;

    public RemoveRoleHandler(
        RoleManager<IdentityRole<Guid>> roleManager,
        IAuditService auditService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<RemoveRoleHandler> logger)
    {
        _roleManager = roleManager;
        _auditService = auditService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(
        RemoveRoleCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing role {RoleName}", request.RoleName);

        var role = await _roleManager.FindByNameAsync(request.RoleName);

        if (role is null)
        {
            return Result<bool>.Failure($"Role '{request.RoleName}' does not exist.", 404);
        }

        if (request.RoleName.Equals("Boss", StringComparison.OrdinalIgnoreCase))
        {
            return Result<bool>.Failure("Cannot remove the Boss role.", 403);
        }

        var result = await _roleManager.DeleteAsync(role);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return Result<bool>.Failure(string.Join("; ", errors), 400);
        }

        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        await _auditService.LogAsync(
            Guid.Empty,
            "RemoveRole",
            "IdentityRole",
            role.Id.ToString(),
            $"Role '{request.RoleName}' existed",
            "Role deleted",
            ipAddress,
            cancellationToken);

        _logger.LogInformation("Successfully removed role {RoleName}", request.RoleName);

        return Result<bool>.Success(true);
    }
}
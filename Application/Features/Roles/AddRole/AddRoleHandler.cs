using Application.Common.Models;
using Domain.Constants;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Features.Roles.AddRole;

public sealed class AddRoleHandler : IRequestHandler<AddRoleCommand, Result<bool>>
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IAuditService _auditService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AddRoleHandler> _logger;

    public AddRoleHandler(
        RoleManager<IdentityRole<Guid>> roleManager,
        IAuditService auditService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AddRoleHandler> logger)
    {
        _roleManager = roleManager;
        _auditService = auditService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(
        AddRoleCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding role {RoleName}", request.RoleName);

if (await _roleManager.RoleExistsAsync(request.RoleName))
        {
return Result<bool>.Failure(
                string.Format(ErrorMessages.RoleAlreadyExistsFormat, request.RoleName),
                HttpStatusCodes.Conflict);
        }

        var role = new IdentityRole<Guid>(request.RoleName)
        {
            Id = Guid.NewGuid()
        };

        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return Result<bool>.Failure(string.Join(ErrorMessages.Separator, errors), HttpStatusCodes.BadRequest);
        }

        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? ErrorMessages.UnknownIp;

        await _auditService.LogAsync(
            Guid.Empty,
            AuditActions.AddRole,
            EntityNames.IdentityRole,
            role.Id.ToString(),
            null,
            $"Role '{request.RoleName}' created",
            ipAddress,
            cancellationToken);

        _logger.LogInformation("Successfully added role {RoleName}", request.RoleName);

        return Result<bool>.Success(true);
    }
}
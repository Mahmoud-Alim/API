using Application.Common.Models;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Features.Roles.PromoteUser;

public sealed class PromoteUserHandler : IRequestHandler<PromoteUserCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IAuditService _auditService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<PromoteUserHandler> _logger;

    public PromoteUserHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IAuditService auditService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<PromoteUserHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _auditService = auditService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(
        PromoteUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Promoting user {UserId} to Admin", request.UserId);

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

if (user is null)
        {
            return Result<bool>.Failure(ErrorMessages.UserNotFound, HttpStatusCodes.NotFound);
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

if (currentRoles.Contains(Domain.Constants.Roles.Boss))
        {
            return Result<bool>.Failure(ErrorMessages.CannotModifyBoss, HttpStatusCodes.Forbidden);
        }

        if (currentRoles.Contains(Domain.Constants.Roles.Admin))
        {
            return Result<bool>.Failure(ErrorMessages.UserAlreadyAdmin, HttpStatusCodes.BadRequest);
        }

        if (!await _roleManager.RoleExistsAsync(Domain.Constants.Roles.Admin))
        {
            return Result<bool>.Failure(ErrorMessages.AdminRoleDoesNotExist, HttpStatusCodes.NotFound);
        }

        var result = await _userManager.AddToRoleAsync(user, Domain.Constants.Roles.Admin);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return Result<bool>.Failure(string.Join(ErrorMessages.Separator, errors), HttpStatusCodes.BadRequest);
        }

        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? ErrorMessages.UnknownIp;

        await _auditService.LogAsync(
            request.UserId,
            AuditActions.PromoteUser,
            EntityNames.ApplicationUser,
            request.UserId.ToString(),
            $"Roles: {string.Join(", ", currentRoles)}",
$"Roles: {string.Join(", ", currentRoles.Append(Domain.Constants.Roles.Admin))}",
            ipAddress,
            cancellationToken);

        _logger.LogInformation("Successfully promoted user {UserId} to Admin", request.UserId);

        return Result<bool>.Success(true);
    }
}
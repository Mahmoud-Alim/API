using Application.Common.Models;
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
            return Result<bool>.Failure("User not found.", 404);
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

        if (currentRoles.Contains("Boss"))
        {
            return Result<bool>.Failure("Cannot modify Boss users.", 403);
        }

        if (currentRoles.Contains("Admin"))
        {
            return Result<bool>.Failure("User is already an Admin.", 400);
        }

        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            return Result<bool>.Failure("Admin role does not exist.", 404);
        }

        var result = await _userManager.AddToRoleAsync(user, "Admin");

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return Result<bool>.Failure(string.Join("; ", errors), 400);
        }

        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        await _auditService.LogAsync(
            request.UserId,
            "PromoteUser",
            "ApplicationUser",
            request.UserId.ToString(),
            $"Roles: {string.Join(", ", currentRoles)}",
            $"Roles: {string.Join(", ", currentRoles.Append("Admin"))}",
            ipAddress,
            cancellationToken);

        _logger.LogInformation("Successfully promoted user {UserId} to Admin", request.UserId);

        return Result<bool>.Success(true);
    }
}
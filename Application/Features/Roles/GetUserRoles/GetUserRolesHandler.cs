using Application.Common.Models;
using Application.Common.Models.Auth;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Features.Roles.GetUserRoles;

public sealed class GetUserRolesHandler : IRequestHandler<GetUserRolesQuery, Result<UserRoleDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GetUserRolesHandler> _logger;

    public GetUserRolesHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<GetUserRolesHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<UserRoleDto>> Handle(
        GetUserRolesQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting roles for user {UserId}", request.UserId);

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
        {
            return Result<UserRoleDto>.Failure("User not found.", 404);
        }

        var roles = await _userManager.GetRolesAsync(user);

        var response = new UserRoleDto
        {
            UserId = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            Roles = roles
        };

        return Result<UserRoleDto>.Success(response);
    }
}
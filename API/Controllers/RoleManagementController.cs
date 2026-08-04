using API.Constants;
using API.Middleware;
using Application.Common.Models.Auth;
using Application.Features.Roles.AddRole;
using Application.Features.Roles.RemoveRole;
using Application.Features.Roles.PromoteUser;
using Application.Features.Roles.GetUserRoles;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route(RouteConstants.Roles.Base)]
[Authorize(Policy = AuthPolicies.RequireAdminRole)]
public sealed class RoleManagementController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(RouteConstants.Roles.PromoteAdmin)]
    [Authorize(Policy = AuthPolicies.RequireBossRole)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> PromoteToAdmin(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var command = new PromoteUserCommand { UserId = userId };
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpPost(RouteConstants.Roles.Add)]
    [Authorize(Policy = AuthPolicies.RequireBossRole)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<bool>> AddRole(
        [FromBody] AddRoleCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpDelete(RouteConstants.Roles.Remove)]
    [Authorize(Policy = AuthPolicies.RequireBossRole)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> RemoveRole(
        [FromBody] RemoveRoleCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpGet(RouteConstants.Roles.GetUserRoles)]
    [Authorize(Policy = AuthPolicies.RequireAdminRole)]
    [ProducesResponseType(typeof(UserRoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserRoleDto>> GetUserRoles(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetUserRolesQuery { UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToActionResult(this);
    }
}

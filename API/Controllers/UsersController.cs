using API.Constants;
using API.Middleware;
using Application.Common.Models;
using Application.Features.Users.AddUser;
using Application.Features.Users.GetActiveUsers;
using Application.Features.Users.GetAllUsers;
using Application.Features.Users.GetUserById;
using Application.Features.Users.GetUserJobInfo;
using Application.Features.Users.GetUserSalary;
using Application.Features.Users.RemoveUser;
using Application.Features.Users.UpdateUser;
using Application.Features.Users.UserExists;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route(RouteConstants.Users.Base)]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedList<UserDto>>> GetAllUsers(
        [FromQuery] GetAllUsersQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpGet(RouteConstants.Users.GetActive)]
    [ProducesResponseType(typeof(IReadOnlyList<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetActiveUsers(CancellationToken cancellationToken)
    {
        var query = new GetActiveUsersQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpGet(RouteConstants.Users.GetById)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUserById(int id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery { Id = id };
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpGet(RouteConstants.Users.GetJobInfo)]
    [ProducesResponseType(typeof(UserJobInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserJobInfoDto>> GetUserJobInfo(int id, CancellationToken cancellationToken)
    {
        var query = new GetUserJobInfoQuery { Id = id };
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpGet(RouteConstants.Users.GetSalary)]
    [ProducesResponseType(typeof(UserSalaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserSalaryDto>> GetUserSalary(int id, CancellationToken cancellationToken)
    {
        var query = new GetUserSalaryQuery { Id = id };
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] AddUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToCreatedActionResult(this, nameof(GetUserById), new { id = result.Data?.UserId });
    }

    [HttpPut(RouteConstants.Users.Update)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        command = command with { Id = id };
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpDelete(RouteConstants.Users.Remove)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveUser(int id, CancellationToken cancellationToken)
    {
        var command = new RemoveUserCommand { Id = id };
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpGet(RouteConstants.Users.UserExists)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> UserExists(int id, CancellationToken cancellationToken)
    {
        var query = new UserExistsQuery { Id = id };
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToActionResult(this);
    }
}

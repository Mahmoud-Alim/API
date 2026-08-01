using Application.Common.Models;
using MediatR;

namespace Application.Features.Users.UpdateUser;

public sealed record UpdateUserCommand : IRequest<Result<UserDto>>
{
    public int Id { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Gender { get; init; } = string.Empty;

    public bool Active { get; init; }
}


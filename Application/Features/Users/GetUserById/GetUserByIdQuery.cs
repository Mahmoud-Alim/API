using Application.Common.Models;
using MediatR;

namespace Application.Features.Users.GetUserById;

public sealed record GetUserByIdQuery : IRequest<Result<UserDto>>
{
    public int Id { get; init; }
}


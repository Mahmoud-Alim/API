using Application.Common.Models;
using MediatR;

namespace Application.Features.Users.RemoveUser;

public sealed record RemoveUserCommand : IRequest<Result<bool>>
{
    public int Id { get; init; }
}


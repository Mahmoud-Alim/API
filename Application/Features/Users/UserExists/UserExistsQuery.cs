using Application.Common.Models;
using MediatR;

namespace Application.Features.Users.UserExists;

public sealed record UserExistsQuery : IRequest<Result<bool>>
{
    public int Id { get; init; }
}


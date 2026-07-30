using Application.Common.Models;
using MediatR;

namespace Application.Features.Roles.PromoteUser;

public sealed record PromoteUserCommand : IRequest<Result<bool>>
{
    public Guid UserId { get; init; }
}
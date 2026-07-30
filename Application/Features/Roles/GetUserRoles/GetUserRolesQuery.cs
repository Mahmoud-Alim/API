using Application.Common.Models;
using Application.Common.Models.Auth;
using MediatR;

namespace Application.Features.Roles.GetUserRoles;

public sealed record GetUserRolesQuery : IRequest<Result<UserRoleDto>>
{
    public Guid UserId { get; init; }
}
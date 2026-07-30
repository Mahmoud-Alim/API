using Application.Common.Models;
using MediatR;

namespace Application.Features.Roles.AddRole;

public sealed record AddRoleCommand : IRequest<Result<bool>>
{
    public string RoleName { get; init; } = string.Empty;
}
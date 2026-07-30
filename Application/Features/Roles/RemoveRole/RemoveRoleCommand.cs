using Application.Common.Models;
using MediatR;

namespace Application.Features.Roles.RemoveRole;

public sealed record RemoveRoleCommand : IRequest<Result<bool>>
{
    public string RoleName { get; init; } = string.Empty;
}
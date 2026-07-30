using Application.Common.Models;
using MediatR;

namespace Application.Features.Users.GetActiveUsers;

public sealed record GetActiveUsersQuery : IRequest<Result<IReadOnlyList<UserDto>>>
{
}

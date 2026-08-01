using Application.Common.Models;
using MediatR;

namespace Application.Features.Users.GetUserSalary;

public sealed record GetUserSalaryQuery : IRequest<Result<UserSalaryDto>>
{
    public int Id { get; init; }
}


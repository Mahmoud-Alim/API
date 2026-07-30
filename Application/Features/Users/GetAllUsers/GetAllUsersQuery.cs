using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.GetAllUsers;

public sealed record GetAllUsersQuery : IRequest<Result<PaginatedList<UserDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; } = "UserId";
    public bool Descending { get; init; } = false;
    public string? SearchTerm { get; init; }
    public string? Gender { get; init; }
    public bool? Active { get; init; }
}

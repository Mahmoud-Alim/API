using Application.Common.Constants;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.GetAllUsers;

public sealed record GetAllUsersQuery : IRequest<Result<PaginatedList<UserDto>>>
{
    public int PageNumber { get; init; } = PaginationConstants.DefaultPageNumber;
    public int PageSize { get; init; } = PaginationConstants.DefaultPageSize;
    public string? SortBy { get; init; } = PaginationConstants.DefaultSortBy;
    public bool Descending { get; init; } = false;
    public string? SearchTerm { get; init; }
    public string? Gender { get; init; }
    public bool? Active { get; init; }
}

using Application.Common.Models;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.GetAllUsers;

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<PaginatedList<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetAllUsersQueryHandler> _logger;

    public GetAllUsersQueryHandler(
        IUserRepository userRepository,
        ILogger<GetAllUsersQueryHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<UserDto>>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting all users - Page: {PageNumber}, PageSize: {PageSize}, SortBy: {SortBy}, Descending: {Descending}, " +
            "SearchTerm: {SearchTerm}, Gender: {Gender}, Active: {Active}",
            request.PageNumber,
            request.PageSize,
            request.SortBy,
            request.Descending,
            request.SearchTerm,
            request.Gender,
            request.Active);

        var filter = new UserFilter
        {
            SearchTerm = request.SearchTerm,
            Gender = request.Gender,
            Active = request.Active
        };

        var (items, totalCount) = await _userRepository.GetAllAsync(
            request.PageNumber,
            request.PageSize,
            request.SortBy,
            request.Descending,
            filter,
            cancellationToken);

        var userDtos = items
            .Select(user => new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender,
                Active = user.Active
            })
            .ToList()
            .AsReadOnly();

        var paginatedList = new PaginatedList<UserDto>(
            userDtos,
            totalCount,
            request.PageNumber,
            request.PageSize);

        _logger.LogInformation(
            "Successfully retrieved {ItemCount} users out of {TotalCount}",
            userDtos.Count,
            totalCount);

        return Result<PaginatedList<UserDto>>.Success(paginatedList);
    }
}

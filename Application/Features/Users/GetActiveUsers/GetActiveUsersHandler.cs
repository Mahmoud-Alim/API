using Application.Common.Models;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.GetActiveUsers;

public sealed class GetActiveUsersHandler : IRequestHandler<GetActiveUsersQuery, Result<IReadOnlyList<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetActiveUsersHandler> _logger;

    public GetActiveUsersHandler(IUserRepository userRepository, ILogger<GetActiveUsersHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<UserDto>>> Handle(
        GetActiveUsersQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all active users");

        var users = await _userRepository.GetActiveAsync(cancellationToken);

        var response = users
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

        _logger.LogInformation(
            "Successfully retrieved {UserCount} active users",
            response.Count);

        return Result<IReadOnlyList<UserDto>>.Success(response);
    }
}


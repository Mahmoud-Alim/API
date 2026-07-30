using Application.Common.Models;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.GetUserSalary;

/// <summary>
/// Handler for <see cref="GetUserSalaryQuery"/> that retrieves a user's salary information.
/// </summary>
public sealed class GetUserSalaryHandler : IRequestHandler<GetUserSalaryQuery, Result<UserSalaryDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserSalaryHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserSalaryHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="logger">The logger instance.</param>
    public GetUserSalaryHandler(IUserRepository userRepository, ILogger<GetUserSalaryHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result<UserSalaryDto>> Handle(
        GetUserSalaryQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting salary for user with Id {UserId}",
            request.Id);

        var userSalary = await _userRepository.GetSingleUserSalaryAsync(
            request.Id,
            cancellationToken);

        if (userSalary is null)
        {
            _logger.LogWarning(
                "Salary information for user {UserId} was not found",
                request.Id);

            return Result<UserSalaryDto>.NotFound(
                $"Salary information for user with Id {request.Id} was not found.");
        }

        var response = new UserSalaryDto
        {
            UserId = userSalary.UserId,
            Salary = userSalary.Salary
        };

        _logger.LogInformation(
            "Successfully retrieved salary for user {UserId}",
            request.Id);

        return Result<UserSalaryDto>.Success(response);
    }
}


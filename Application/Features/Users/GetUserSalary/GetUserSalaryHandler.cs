using Application.Common.Models;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.GetUserSalary;

public sealed class GetUserSalaryHandler : IRequestHandler<GetUserSalaryQuery, Result<UserSalaryDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserSalaryHandler> _logger;

    public GetUserSalaryHandler(IUserRepository userRepository, ILogger<GetUserSalaryHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

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


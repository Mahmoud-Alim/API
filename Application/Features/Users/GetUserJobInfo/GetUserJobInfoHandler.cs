using Application.Common.Models;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.GetUserJobInfo;

/// <summary>
/// Handler for <see cref="GetUserJobInfoQuery"/> that retrieves a user's job information.
/// </summary>
public sealed class GetUserJobInfoHandler : IRequestHandler<GetUserJobInfoQuery, Result<UserJobInfoDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserJobInfoHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserJobInfoHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="logger">The logger instance.</param>
    public GetUserJobInfoHandler(IUserRepository userRepository, ILogger<GetUserJobInfoHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result<UserJobInfoDto>> Handle(
        GetUserJobInfoQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting job information for user with Id {UserId}",
            request.Id);

        var userJobInfo = await _userRepository.GetSingleUserJobInfoAsync(
            request.Id,
            cancellationToken);

        if (userJobInfo is null)
        {
            _logger.LogWarning(
                "Job information for user {UserId} was not found",
                request.Id);

            return Result<UserJobInfoDto>.NotFound(
                $"Job information for user with Id {request.Id} was not found.");
        }

        var response = new UserJobInfoDto
        {
            UserId = userJobInfo.UserId,
            JobTitle = userJobInfo.JobTitle,
            Department = userJobInfo.Department
        };

        _logger.LogInformation(
            "Successfully retrieved job information for user {UserId}",
            request.Id);

        return Result<UserJobInfoDto>.Success(response);
    }
}


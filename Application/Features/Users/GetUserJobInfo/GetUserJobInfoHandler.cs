using Application.Common.Models;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.GetUserJobInfo;

public sealed class GetUserJobInfoHandler : IRequestHandler<GetUserJobInfoQuery, Result<UserJobInfoDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserJobInfoHandler> _logger;

    public GetUserJobInfoHandler(IUserRepository userRepository, ILogger<GetUserJobInfoHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

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


using Application.Common.Models;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.UserExists;

public sealed class UserExistsHandler : IRequestHandler<UserExistsQuery, Result<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserExistsHandler> _logger;

    public UserExistsHandler(IUserRepository userRepository, ILogger<UserExistsHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(
        UserExistsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Checking whether user {UserId} exists",
            request.Id);

        var exists = await _userRepository.ExistsAsync(
            request.Id,
            cancellationToken);

        _logger.LogInformation(
            "User {UserId} existence check result: {Exists}",
            request.Id,
            exists);

        return Result<bool>.Success(exists);
    }
}


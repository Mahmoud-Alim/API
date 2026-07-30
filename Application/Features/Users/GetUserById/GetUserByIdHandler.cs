using Application.Common.Models;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.GetUserById;

public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserByIdHandler> _logger;

    public GetUserByIdHandler(IUserRepository userRepository, ILogger<GetUserByIdHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<UserDto>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting user with Id {UserId}",
            request.Id);

        var user = await _userRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (user is null)
        {
            _logger.LogWarning(
                "User {UserId} was not found",
                request.Id);

            return Result<UserDto>.NotFound($"User with Id {request.Id} was not found.");
        }

        var response = new UserDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Gender = user.Gender,
            Active = user.Active
        };

        _logger.LogInformation(
            "Successfully retrieved user {UserId}",
            request.Id);

        return Result<UserDto>.Success(response);
    }
}


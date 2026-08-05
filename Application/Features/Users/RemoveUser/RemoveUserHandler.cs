using Application.Common.Models;
using Domain.Constants;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.RemoveUser;

public sealed class RemoveUserHandler : IRequestHandler<RemoveUserCommand, Result<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RemoveUserHandler> _logger;

    public RemoveUserHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<RemoveUserHandler> logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(
        RemoveUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Removing user with Id {UserId}",
            request.Id);

        var user = await _userRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (user is null)
        {
            _logger.LogWarning(
                "User {UserId} was not found for removal",
                request.Id);

return Result<bool>.NotFound(
                string.Format(ErrorMessages.UserWithIdNotFoundFormat, request.Id));
        }

        _userRepository.Remove(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Successfully removed user {UserId}",
            request.Id);

        return Result<bool>.Success(true);
    }
}


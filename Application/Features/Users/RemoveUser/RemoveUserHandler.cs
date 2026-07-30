using Application.Common.Models;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.RemoveUser;

/// <summary>
/// Handler for <see cref="RemoveUserCommand"/> that removes an existing user.
/// </summary>
public sealed class RemoveUserHandler : IRequestHandler<RemoveUserCommand, Result<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RemoveUserHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveUserHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="unitOfWork">The unit of work for transactional integrity.</param>
    /// <param name="logger">The logger instance.</param>
    public RemoveUserHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<RemoveUserHandler> logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
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
                $"User with Id {request.Id} was not found.");
        }

        _userRepository.Remove(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Successfully removed user {UserId}",
            request.Id);

        return Result<bool>.Success(true);
    }
}


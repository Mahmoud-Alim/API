using Application.Common.Models;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.AddUser;

public sealed class AddUserHandler : IRequestHandler<AddUserCommand, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddUserHandler> _logger;

    public AddUserHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddUserHandler> logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UserDto>> Handle(
        AddUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Adding new user with email {Email}",
            request.Email);

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Gender = request.Gender,
            Active = request.Active
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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
            "Successfully added user with Id {UserId}",
            user.UserId);

        return Result<UserDto>.Success(response);
    }
}


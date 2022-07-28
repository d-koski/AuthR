using AuthR.BusinessLogic.Abstractions.Services;
using AuthR.BusinessLogic.Models.Commands.User;
using AuthR.BusinessLogic.Models.Exceptions;
using AuthR.BusinessLogic.Models.ViewModels;
using AuthR.DataAccess.Abstractions;
using AuthR.DataAccess.Abstractions.Repositories;
using AuthR.DataAccess.Entities;
using MediatR;

namespace AuthR.BusinessLogic.Handlers.User;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserViewModel>
{
    private readonly IHashingService _hashingService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(
        IHashingService hashingService,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository)
    {
        _hashingService = hashingService;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }
    
    public async Task<UserViewModel> Handle(RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        var exists = await _userRepository.ExistsAsync(command.Email, cancellationToken);
        if (exists)
            throw new EntityExistsException("UserEmailAlreadyRegistered");
        
        var passwordHash = _hashingService.HashPassword(command.Password);
        var entity = new UserEntity
        {
            Email = command.Email,
            PasswordHash = passwordHash,
        };
        
        await _userRepository.InsertAsync(entity, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var result = new UserViewModel
        {
            Guid = entity.Guid,
            Email = command.Email,
        };
        return result;
    }
}
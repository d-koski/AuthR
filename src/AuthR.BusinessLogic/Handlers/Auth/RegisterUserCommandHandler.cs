using System.Security.Cryptography;
using AuthR.BusinessLogic.Exceptions;
using AuthR.BusinessLogic.Models.Commands;
using AuthR.BusinessLogic.Models.ViewModels;
using AuthR.DataAccess.Abstractions;
using AuthR.DataAccess.Abstractions.Repositories;
using AuthR.DataAccess.Entities;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace AuthR.BusinessLogic.Handlers.Auth;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }
    
    public async Task<UserViewModel> Handle(RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        var exists = await _userRepository.ExistsAsync(command.Email, cancellationToken);
        if (exists)
            throw new EntityExistsException("A user with the provided email is already registered.");
        
        var passwordHash = HashPassword(command.Password);
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

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(32);
        var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 64000,
            numBytesRequested: 192));
        return hash;
    }
}
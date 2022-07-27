using System.Security.Authentication;
using AuthR.BusinessLogic.Abstractions.Services;
using AuthR.BusinessLogic.Exceptions;
using AuthR.BusinessLogic.Models.Commands;
using AuthR.BusinessLogic.Models.ViewModels;
using AuthR.DataAccess.Abstractions;
using AuthR.DataAccess.Abstractions.Repositories;
using MediatR;

namespace AuthR.BusinessLogic.Handlers.User;

public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, TokensViewModel>
{
    private readonly IHashingService _hashingService;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;

    public AuthenticateUserCommandHandler(
        IHashingService hashingService,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository)
    {
        _hashingService = hashingService;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
    }
    
    public async Task<TokensViewModel> Handle(
        AuthenticateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (user is null)
            throw new EntityNotFoundException("UserEmailNotFound");

        var correctPassword = _hashingService.VerifyPassword(user.PasswordHash, command.Password);
        if (!correctPassword)
            throw new AuthenticationException("UserInvalidPassword");

        var refreshTokenEntity = await _refreshTokenRepository.GenerateAsync(cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var accessToken = _tokenService.NewAccessToken(user.Guid, user.Email);
        var refreshToken = _tokenService.NewRefreshToken(refreshTokenEntity.Guid, user.Guid, user.Email);
        return new TokensViewModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }
}
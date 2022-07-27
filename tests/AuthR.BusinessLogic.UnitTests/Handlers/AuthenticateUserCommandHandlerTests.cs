using System.Security.Authentication;
using AuthR.BusinessLogic.Abstractions.Services;
using AuthR.BusinessLogic.Exceptions;
using AuthR.BusinessLogic.Handlers.User;
using AuthR.BusinessLogic.Models.Commands;
using AuthR.DataAccess.Abstractions;
using AuthR.DataAccess.Abstractions.Repositories;
using AuthR.DataAccess.Entities;

namespace AuthR.BusinessLogic.UnitTests.Handlers;

public class AuthenticateUserCommandHandlerTests
{
    private readonly Mock<IHashingService> _hashingServiceMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private readonly AuthenticateUserCommandHandler _handler;

    public AuthenticateUserCommandHandlerTests()
    {
        _handler = new AuthenticateUserCommandHandler(
            _hashingServiceMock.Object,
            _tokenServiceMock.Object,
            _unitOfWorkMock.Object,
            _refreshTokenRepositoryMock.Object,
            _userRepositoryMock.Object);
    }

    [Fact]
    public async void Handle_ThrowsEntityNotFoundException_WhenUserWasNotFound()
    {
        const string expectedExceptionMessage = "UserEmailNotFound";

        var command = new AuthenticateUserCommand();

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserEntity?)null);

        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _handler.Handle(command));
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }

    [Fact]
    public async void Handle_ThrowsAuthenticationException_ProvidedPasswordIsInvalid()
    {
        const string expectedExceptionMessage = "UserInvalidPassword";

        var command = new AuthenticateUserCommand();

        var userEntity = new UserEntity();

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userEntity);

        _hashingServiceMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        var exception = await Assert.ThrowsAsync<AuthenticationException>(() => _handler.Handle(command));
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }
}
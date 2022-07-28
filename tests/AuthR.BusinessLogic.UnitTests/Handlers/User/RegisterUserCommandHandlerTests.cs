using AuthR.BusinessLogic.Abstractions.Services;
using AuthR.BusinessLogic.Exceptions;
using AuthR.BusinessLogic.Handlers.User;
using AuthR.BusinessLogic.Models.Commands.User;
using AuthR.DataAccess.Abstractions;
using AuthR.DataAccess.Abstractions.Repositories;
using AuthR.DataAccess.Entities;

namespace AuthR.BusinessLogic.UnitTests.Handlers.User;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IHashingService> _hashingServiceMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private readonly RegisterUserCommandHandler _handler;
    
    public RegisterUserCommandHandlerTests()
    {
        _handler = new RegisterUserCommandHandler(
            _hashingServiceMock.Object,
            _unitOfWorkMock.Object,
            _userRepositoryMock.Object);
    }

    [Fact]
    public async void Handle_ValidUser_ThrowsExceptionWhenUserWithGivenEmailAlreadyExists()
    {
        var command = new RegisterUserCommand();

        _userRepositoryMock.Setup(x => x.ExistsAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<EntityExistsException>(() => _handler.Handle(command));
    }

    [Fact]
    public async void Handle_ValidUser_DoesntInsertPasswordDirectlyIntoDatabase()
    {
        var command = new RegisterUserCommand
        {
            Password = "Test123!"
        };

        _userRepositoryMock.Setup(x => x.ExistsAsync(
                command.Email,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await _handler.Handle(command);
        
        _userRepositoryMock.Verify(x => x.InsertAsync(
                It.Is<UserEntity>(e => e.PasswordHash != command.Password),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
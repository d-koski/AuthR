using AuthR.BusinessLogic.Abstractions.Services;
using AuthR.BusinessLogic.Exceptions;
using AuthR.BusinessLogic.Handlers.Token;
using AuthR.BusinessLogic.Models.Commands.Token;
using AuthR.DataAccess.Abstractions;
using AuthR.DataAccess.Abstractions.Repositories;
using AuthR.DataAccess.Entities;

namespace AuthR.BusinessLogic.UnitTests.Handlers.Token;

public class RevokeRefreshTokenCommandHandlerTests
{
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository = new();

    private readonly RevokeRefreshTokenCommandHandler _handler;

    public RevokeRefreshTokenCommandHandlerTests()
    {
        _handler = new RevokeRefreshTokenCommandHandler(
            _tokenService.Object,
            _unitOfWork.Object,
            _refreshTokenRepository.Object);
    }

    // Untracked refresh token means that no record with the token GUID exist in the database. Because the token was
    // verified earlier (and the signature was correct), the token secret was probably compromised.
    [Fact]
    public async void Handle_UntrackedRefreshToken_ThrowsInvalidTokenException()
    {
        const string expectedExceptionMessage = "InvalidRefreshToken";

        const string refreshToken = "fake-refresh-token";
        var command = new RevokeRefreshTokenCommand(refreshToken);

        var tokenGuid = new Guid("bd477c5f-935e-4061-b2d8-b15cfaee0de4");

        _tokenService.Setup(x => x.GetRefreshTokenGuid(refreshToken))
            .Returns(tokenGuid);

        _refreshTokenRepository.Setup(x => x.GetByGuidAsync(tokenGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync((RefreshTokenEntity?)null);

        var exception = await Assert.ThrowsAsync<InvalidTokenException>(() => _handler.Handle(command));
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }

    [Fact]
    public async void Handle_ValidRefreshToken_RevokesToken()
    {
        const string refreshToken = "fake-refresh-token";
        var command = new RevokeRefreshTokenCommand(refreshToken);

        var tokenGuid = new Guid("bd477c5f-935e-4061-b2d8-b15cfaee0de4");

        var tokenEntity = new RefreshTokenEntity();

        _tokenService.Setup(x => x.GetRefreshTokenGuid(refreshToken))
            .Returns(tokenGuid);

        _refreshTokenRepository.Setup(x => x.GetByGuidAsync(tokenGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokenEntity);

        _ = await _handler.Handle(command);

        _refreshTokenRepository.Verify(x => x.Revoke(tokenEntity), Times.Once);
    }
}
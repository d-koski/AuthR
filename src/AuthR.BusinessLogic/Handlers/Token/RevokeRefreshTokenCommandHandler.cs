using AuthR.BusinessLogic.Abstractions.Services;
using AuthR.BusinessLogic.Models.Commands.Token;
using AuthR.BusinessLogic.Models.Exceptions;
using AuthR.DataAccess.Abstractions;
using AuthR.DataAccess.Abstractions.Repositories;
using MediatR;

namespace AuthR.BusinessLogic.Handlers.Token;

public class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand, Unit>
{
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RevokeRefreshTokenCommandHandler(
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Unit> Handle(RevokeRefreshTokenCommand command, CancellationToken cancellationToken = default)
    {
        var tokenGuid = _tokenService.GetRefreshTokenGuid(command.RefreshToken);

        var tokenEntity = await _refreshTokenRepository.GetByGuidAsync(tokenGuid, cancellationToken);
        if (tokenEntity is null)
            throw new InvalidTokenException("InvalidRefreshToken");

        _refreshTokenRepository.Revoke(tokenEntity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
using MediatR;

namespace AuthR.BusinessLogic.Models.Commands.Token;

public record RevokeRefreshTokenCommand(string RefreshToken) : IRequest<Unit>;
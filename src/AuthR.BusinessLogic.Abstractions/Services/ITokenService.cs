namespace AuthR.BusinessLogic.Abstractions.Services;

public interface ITokenService
{
    string NewAccessToken(Guid userGuid, string userEmail);

    string NewRefreshToken(Guid tokenGuid, Guid userGuid, string userEmail);

    Guid GetRefreshTokenGuid(string refreshToken);
}
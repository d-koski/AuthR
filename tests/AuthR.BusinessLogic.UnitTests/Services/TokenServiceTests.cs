using System.IdentityModel.Tokens.Jwt;
using AuthR.BusinessLogic.Services;
using AuthR.Common.Abstractions.Services;

namespace AuthR.BusinessLogic.UnitTests.Services;

public class TokenServiceTests
{
    private readonly Mock<IDateTimeService> _dateTimeServiceMock = new();

    private readonly TokenService _service;

    private static readonly JwtSecurityTokenHandler JwtHandler = new();

    public TokenServiceTests()
    {
        _service = new TokenService(_dateTimeServiceMock.Object);
    }

    [Fact]
    public void NewAccessToken_GeneratedToken_HasCorrectSubClaim()
    {
        var userGuid = new Guid("935770bf-6fb7-460e-b2b0-6ff07c34c1b9");
        const string userEmail = "test@test.com";

        var accessToken = _service.NewAccessToken(userGuid, userEmail);

        var securityToken = JwtHandler.ReadJwtToken(accessToken);
        var subjectClaim = securityToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub);
        Assert.Equal(userGuid.ToString(), subjectClaim.Value);
    }

    [Fact]
    public void NewAccessToken_GeneratedToken_HasCorrectEmailClaim()
    {
        var userGuid = new Guid("935770bf-6fb7-460e-b2b0-6ff07c34c1b9");
        const string userEmail = "test@test.com";

        var accessToken = _service.NewAccessToken(userGuid, userEmail);

        var securityToken = JwtHandler.ReadJwtToken(accessToken);
        var emailClaim = securityToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email);
        Assert.Equal(userEmail, emailClaim.Value);
    }

    [Fact]
    public void NewRefreshToken_GeneratedToken_HasCorrectJtiClaim()
    {
        var tokenGuid = new Guid("4dc90f87-eed9-40ad-aae8-f893c246f10e");
        var userGuid = new Guid("935770bf-6fb7-460e-b2b0-6ff07c34c1b9");
        const string userEmail = "test@test.com";

        var refreshToken = _service.NewRefreshToken(tokenGuid, userGuid, userEmail);

        var securityToken = JwtHandler.ReadJwtToken(refreshToken);
        var tokenIdClaim = securityToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti);
        Assert.Equal(tokenGuid.ToString(), tokenIdClaim.Value);
    }

    [Fact]
    public void NewRefreshToken_GeneratedToken_HasCorrectSubClaim()
    {
        var tokenGuid = new Guid("4dc90f87-eed9-40ad-aae8-f893c246f10e");
        var userGuid = new Guid("935770bf-6fb7-460e-b2b0-6ff07c34c1b9");
        const string userEmail = "test@test.com";

        var refreshToken = _service.NewRefreshToken(tokenGuid, userGuid, userEmail);

        var securityToken = JwtHandler.ReadJwtToken(refreshToken);
        var subjectClaim = securityToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub);
        Assert.Equal(userGuid.ToString(), subjectClaim.Value);
    }

    [Fact]
    public void NewRefreshToken_GeneratedToken_HasCorrectEmailClaim()
    {
        var tokenGuid = new Guid("4dc90f87-eed9-40ad-aae8-f893c246f10e");
        var userGuid = new Guid("935770bf-6fb7-460e-b2b0-6ff07c34c1b9");
        const string userEmail = "test@test.com";

        var refreshToken = _service.NewRefreshToken(tokenGuid, userGuid, userEmail);

        var securityToken = JwtHandler.ReadJwtToken(refreshToken);
        var emailClaim = securityToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email);
        Assert.Equal(userEmail, emailClaim.Value);
    }
}
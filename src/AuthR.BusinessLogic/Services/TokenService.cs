using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthR.BusinessLogic.Abstractions.Services;
using AuthR.BusinessLogic.Exceptions;
using AuthR.Common.Abstractions.Services;
using Microsoft.IdentityModel.Tokens;

namespace AuthR.BusinessLogic.Services;

public class TokenService : ITokenService
{
    private const string AccessPrivateKey = @"MIIEogIBAAKCAQB+D6ZM306czniISDJL2rjRan1ks6MzjaDLooZ+wrHSvYYBP8pu
TxIeoS4Rmz5K6VkDGUZY6ZzL1ZLhFblQq0yTUq5MRbNzBcCIieXDqAuQaho8IlPg
MopdbJXfIIq5u2wrJ2vwARUjc0LH4lTyPxQM7Czg7qASPGFPhyclGFSHq/5MRYRI
RUiXMRnD+GE78jFOSXU+fM0Vhnj1Xep1vx2NS9ZqAflGbY/GBfetot5wkUXmrT+W
LmPDOA2ENnBAoEOWKrxjjFNd2AoVmgxlaDrnHSiMaF662W26bCZOEecqDnLIko91
logKuW9YxH6Ha0Y5HaaJiGCK2s2zTcr3TLhXAgMBAAECggEAXA4tkOFZVOTp3Ats
vzvfzv2GyCzuNs1r6Iis0pB2rsA7xVnCB7+yKa/mJnFJkgbJO5wPZQpjt+4krj3g
7+nHp6WvJn+XxZ0jthGNOc3TdAgnVFZ1DbmalRKgdyoaY/tbyD+ncMI5n4Oh9rlu
8t/hUnZ9Z1yag/aX0S2PBmW4BMIhjkU1Prlp2DGIyCmpFMyND2akFiAcZSi2/QS+
dfpr88MwaVflf3+eXMpJ7arcHuCVBJkcC70l6TY2Ps09a3aJ07tEjJVJPXv70+LX
bUVsEVWrPlFk5s8RrSQ9ZA1X6MoEU52yvyG5L/S7AVvIaYgaTMjRAH3HerohyE7E
zg5c8QKBgQDusBdZp9tnHgf1CDtQsyAYNJXhnFLaxnTy8SUolwlkn0F3LfaE9tCR
uEdi2zyl9TWQWc6LpZKcpOxA1XOiXVNiAd+XD4iDv58WOkXgO9+r/vdkQ087QN2D
FUgs1skpBCrbQQ45MpmrV9Wce6G1/awNJZKaEQq/9NCb/z61SAriHQKBgQCHNFPk
MHPDURwLJQSu43K2bhoiLHh65e4fOn7l85aXNTBcXVNXr38MbhcKt7EsljUOhZgq
WuESmo5ueoNcr2yl9MPdNi2cOtxUicLh4htf7+p5PKcTyqvAYrcSzwcel7V0bh53
NUJgyCtY9L4quxsygLllHCOBPMD2If3rLP+6AwKBgQDr7ktmZ7FLgDfFUNmwrx5u
TqxIUcjkT36SFSLxuDmkqHaBY4FldMI9B7YoVSThtju3781l48fD5pFQlKy0NSOS
oN9t5gC+mc6angcr2oMA9Al9pbrPixJp28N37ubwEBp8lxWEHCwhXbTb9Kinx5Ml
e5oph+bkTw1U5TwK3KdFUQKBgDttbqpP+bISBqeHX+WXJ6a0Alye+13Zjq6/QWPF
i51uzZDrnGRgSRGnmg0l7IRSiYOWiOmlBGTu9kftJawplzHiweyLkcSnwoyN+NDc
V3f1tjQPyshSPufS+/ESemih+inw9Qckq8ZqdVMmUCfsEepC1jpxAdIeaBpHmpoy
Lz0NAoGATQfBaoJ23g9r8lSrLBfD7Y62YIDkgK7Tz5Lmyj9G0m8yMTfQTQ5pOnCE
JyejD16tPopSDPEjQva3yMapruCQ9p5d2uZLpA2dJxrkcaVk/vwJNTe6G3SkGDex
U33pkxUsrrQ6YL2fNxfw6+lBmFyoXXMxe0CJg0IJdbLieICSgY8=";

    private const string AccessPublicKey = @"MIIBITANBgkqhkiG9w0BAQEFAAOCAQ4AMIIBCQKCAQB+D6ZM306czniISDJL2rjR
an1ks6MzjaDLooZ+wrHSvYYBP8puTxIeoS4Rmz5K6VkDGUZY6ZzL1ZLhFblQq0yT
Uq5MRbNzBcCIieXDqAuQaho8IlPgMopdbJXfIIq5u2wrJ2vwARUjc0LH4lTyPxQM
7Czg7qASPGFPhyclGFSHq/5MRYRIRUiXMRnD+GE78jFOSXU+fM0Vhnj1Xep1vx2N
S9ZqAflGbY/GBfetot5wkUXmrT+WLmPDOA2ENnBAoEOWKrxjjFNd2AoVmgxlaDrn
HSiMaF662W26bCZOEecqDnLIko91logKuW9YxH6Ha0Y5HaaJiGCK2s2zTcr3TLhX
AgMBAAE=";

    private const string RefreshSecret = "Xn2r5u8x/A?D(G+KbPdSgVkYp3s6v9y$B&E)H@McQfThWmZq4t7w!z%C*F-JaNdR";

    private const string Issuer = "authr";
    private const string Audience = "audience";

    private const int AccessExpiresInMinutes = 10;
    private const int RefreshExpiresInDays = 7;

    private static readonly JwtSecurityTokenHandler JwtHandler = new();

    private readonly IDateTimeService _dateTimeService;
    
    public TokenService(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public string NewAccessToken(Guid userGuid, string userEmail)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, userGuid.ToString()),
            new(JwtRegisteredClaimNames.Email, userEmail)
        };

        using var rsaKey = RSA.Create();
        rsaKey.ImportRSAPrivateKey(Convert.FromBase64String(AccessPrivateKey), out _);
        var securityKey = NewRsaSecurityKey(rsaKey);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);

        var securityToken = new JwtSecurityToken(
            Issuer,
            Audience,
            claims,
            _dateTimeService.Now,
            _dateTimeService.Now.AddMinutes(AccessExpiresInMinutes),
            signingCredentials);

        var token = JwtHandler.WriteToken(securityToken);
        return token;
    }

    public string NewRefreshToken(Guid tokenGuid, Guid userGuid, string userEmail)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Jti, tokenGuid.ToString()),
            new(JwtRegisteredClaimNames.Sub, userGuid.ToString()),
            new(JwtRegisteredClaimNames.Email, userEmail)
        };

        var secretBytes = Encoding.ASCII.GetBytes(RefreshSecret);
        var securityKey = new SymmetricSecurityKey(secretBytes);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var securityToken = new JwtSecurityToken(
            Issuer,
            Audience,
            claims,
            _dateTimeService.Now,
            _dateTimeService.Now.AddDays(RefreshExpiresInDays),
            signingCredentials);

        var token = JwtHandler.WriteToken(securityToken);
        return token;
    }

    public Guid GetRefreshTokenGuid(string refreshToken)
    {
        try
        {
            var principal = JwtHandler.ValidateToken(refreshToken, GetRefreshTokenValidationParameters(), out _);
            var guidValue = principal.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var guid = new Guid(guidValue);
            return guid;
        }
        catch (SecurityTokenException)
        {
            throw new InvalidTokenException("InvalidRefreshToken");
        }
    }

    private static RsaSecurityKey NewRsaSecurityKey(RSA rsa)
    {
        var securityKey = new RsaSecurityKey(rsa)
        {
            CryptoProviderFactory = new CryptoProviderFactory
            {
                CacheSignatureProviders = false
            }
        };
        return securityKey;
    }

    private static TokenValidationParameters GetRefreshTokenValidationParameters()
    {
        var secretBytes = Encoding.ASCII.GetBytes(RefreshSecret);
        var securityKey = new SymmetricSecurityKey(secretBytes);
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = securityKey,
            CryptoProviderFactory = new CryptoProviderFactory
            {
                CacheSignatureProviders = false
            }
        };
    }
}
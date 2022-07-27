using AuthR.BusinessLogic.Services;

namespace AuthR.BusinessLogic.UnitTests.Services;

public class HashingServiceTests
{
    private readonly HashingService _service = new();

    [Fact]
    public void HashPassword_NotEmpty_ReturnsHashWithLength512()
    {
        const string password = "Test123!";

        var result = _service.HashPassword(password);
        
        Assert.Equal(512, result.Length);
    }

    [Fact]
    public void VerifyPassword_NotEmpty_ReturnsTrueWhenPasswordAreSame()
    {
        const string password = "Test123!";
        
        var passwordHash = _service.HashPassword(password);

        var result = _service.VerifyPassword(passwordHash, password);
        
        Assert.True(result);
    }
}
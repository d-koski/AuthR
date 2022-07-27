using AuthR.Api.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthR.Api.UnitTests.DependencyInjection;

public class ApiServiceCollectionExtensionsTests
{
    private readonly Mock<IConfiguration> _configurationMock = new();

    [Fact]
    public void AddApi_AddsBusinessLogicDI()
    {
        var services = new ServiceCollection();
        services.AddApi(_configurationMock.Object);

        var result = services.Select(x => x.ServiceType)
            .Any(x => x.Namespace?.StartsWith("AuthR.BusinessLogic") == true);

        Assert.True(result);
    }
}
using AuthR.BusinessLogic.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthR.BusinessLogic.UnitTests.DependencyInjection;

public class BusinessLogicServiceCollectionExtensionsTests
{
    private readonly Mock<IConfiguration> _configurationMock = new();

    [Fact]
    public void AddApi_AddsCommonDI()
    {
        var services = new ServiceCollection();
        services.AddBusinessLogic(_configurationMock.Object);

        var result = services.Select(x => x.ServiceType)
            .Any(x => x.Namespace?.StartsWith("AuthR.Common") == true);

        Assert.True(result);
    }

    [Fact]
    public void AddApi_AddsDataAccessDI()
    {
        var services = new ServiceCollection();
        services.AddBusinessLogic(_configurationMock.Object);

        var result = services.Select(x => x.ServiceType)
            .Any(x => x.Namespace?.StartsWith("AuthR.DataAccess") == true);

        Assert.True(result);
    }
}
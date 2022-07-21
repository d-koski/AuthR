using AuthR.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace AuthR.BusinessLogic;

public static class DependencyInjectionConfiguration
{
    public static void AddBusinessLogic(this IServiceCollection services)
    {
        services.AddDataAccess();
    }
}
using System.Reflection;
using AuthR.DataAccess;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthR.BusinessLogic;

public static class DependencyInjectionConfiguration
{
    public static void AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        services.AddAutoMapper(executingAssembly);
        services.AddMediatR(executingAssembly);
        
        services.AddDataAccess(configuration);
    }
}
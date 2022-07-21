using System.Reflection;
using AuthR.DataAccess;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AuthR.BusinessLogic;

public static class DependencyInjectionConfiguration
{
    public static void AddBusinessLogic(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        
        services.AddDataAccess();
    }
}
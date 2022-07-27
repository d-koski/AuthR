using System.Reflection;
using AuthR.BusinessLogic.Abstractions.Services;
using AuthR.BusinessLogic.Services;
using AuthR.Common;
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

        services.AddCommons();
        services.AddDataAccess(configuration);

        services.AddScoped<IHashingService, HashingService>();
        services.AddScoped<ITokenService, TokenService>();
    }
}
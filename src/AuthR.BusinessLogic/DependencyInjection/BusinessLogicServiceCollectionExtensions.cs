using System.Reflection;
using AuthR.BusinessLogic.Abstractions.Services;
using AuthR.BusinessLogic.Services;
using AuthR.Common.DependencyInjection;
using AuthR.DataAccess.DependencyInjection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthR.BusinessLogic.DependencyInjection;

public static class BusinessLogicServiceCollectionExtensions
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
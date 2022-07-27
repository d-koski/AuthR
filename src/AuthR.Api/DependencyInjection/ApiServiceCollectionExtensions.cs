using System.Reflection;
using AuthR.Api.Middlewares;
using AuthR.BusinessLogic.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace AuthR.Api.DependencyInjection;

public static class ApiServiceCollectionExtensions
{
    public static void AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(executingAssembly);
        services.AddFluentValidationAutoValidation();

        services.AddBusinessLogic(configuration);

        services.AddScoped<ExceptionHandlerMiddleware>();
    }
}
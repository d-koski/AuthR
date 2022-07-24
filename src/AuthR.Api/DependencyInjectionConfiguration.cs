using System.Reflection;
using AuthR.Api.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace AuthR.Api;

public static class DependencyInjectionConfiguration
{
    public static void AddApi(this IServiceCollection services)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(executingAssembly);
        services.AddFluentValidationAutoValidation();

        services.AddScoped<ExceptionHandlerMiddleware>();
    }

    public static void UseMiddlewares(this WebApplication application)
    {
        application.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
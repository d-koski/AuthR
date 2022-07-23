using System.Reflection;
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
    }
}
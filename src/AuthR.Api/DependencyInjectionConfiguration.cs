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

    public static void UseLocalization(this WebApplication application)
    {
        var supportedCultures = new[] { "en-US", "pl-PL" };
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);
        localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
        application.UseRequestLocalization(localizationOptions);
    }

    public static void UseMiddlewares(this WebApplication application)
    {
        application.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
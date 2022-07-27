namespace AuthR.Api.DependencyInjection;

public static class LocalizationWebApplicationExtensions
{
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
}
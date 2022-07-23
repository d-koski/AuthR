using AuthR.Common.Abstractions.Services;
using AuthR.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthR.Common;

public static class DependencyInjectionConfiguration
{
    public static void AddCommons(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeService, DateTimeService>();
    }
}
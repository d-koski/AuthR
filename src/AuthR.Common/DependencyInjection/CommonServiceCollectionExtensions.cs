using AuthR.Common.Abstractions.Services;
using AuthR.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthR.Common.DependencyInjection;

public static class CommonServiceCollectionExtensions
{
    public static void AddCommons(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeService, DateTimeService>();
    }
}
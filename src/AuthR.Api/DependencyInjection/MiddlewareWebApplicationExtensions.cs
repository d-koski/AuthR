using AuthR.Api.Middlewares;

namespace AuthR.Api;

public static class MiddlewareWebApplicationExtensions
{
    public static void UseMiddlewares(this WebApplication application)
    {
        application.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
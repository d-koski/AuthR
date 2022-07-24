using System.Net.Mime;
using AuthR.BusinessLogic.Exceptions;
using AuthR.BusinessLogic.Models.ViewModels;

namespace AuthR.Api.Middlewares;

public class ExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            EntityExistsException ex => (StatusCodes.Status409Conflict, new ErrorViewModel(ex.Message)),
            _ => (StatusCodes.Status500InternalServerError, new ErrorViewModel("Internal server error."))
        };
        
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(message);
    }
}
using System.Net.Mime;
using System.Security.Authentication;
using AuthR.Api.Localization;
using AuthR.BusinessLogic.Exceptions;
using AuthR.BusinessLogic.Models.ViewModels;
using Microsoft.Extensions.Localization;

namespace AuthR.Api.Middlewares;

public class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly IStringLocalizer<LocalizationResources> _localizer;

    public ExceptionHandlerMiddleware(IStringLocalizer<LocalizationResources> localizer)
    {
        _localizer = localizer;
    }
    
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            AuthenticationException ex => (StatusCodes.Status401Unauthorized, new ErrorViewModel(_localizer[ex.Message])),
            EntityExistsException ex => (StatusCodes.Status409Conflict, new ErrorViewModel(_localizer[ex.Message])),
            EntityNotFoundException ex => (StatusCodes.Status404NotFound, new ErrorViewModel(_localizer[ex.Message])),
            _ => (StatusCodes.Status500InternalServerError, new ErrorViewModel(_localizer["InternalServerError"])),
        };
        
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(message);
    }
}
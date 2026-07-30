using System.Net;
using System.Text.Json;
using HRMS.Shared.Exceptions;
using HRMS.Shared.Responses;

namespace HRMS.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,

            NotFoundException => HttpStatusCode.NotFound,

            ConflictException => HttpStatusCode.Conflict,

            UnauthorizedException => HttpStatusCode.Unauthorized,

            BusinessException => HttpStatusCode.BadRequest,

            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)statusCode;

     var response = new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = statusCode == HttpStatusCode.InternalServerError
                ? "An unexpected error occurred."
                : exception.Message,
            TraceId = context.TraceIdentifier,
            Errors = exception is ValidationException validationException
                ? validationException.Errors
                : null
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}
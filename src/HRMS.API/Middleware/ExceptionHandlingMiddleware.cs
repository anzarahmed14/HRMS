using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.API.Responses;

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
            if (ex is BusinessException ||
                ex is ConflictException ||
                ex is NotFoundException ||
                ex is HRMS.BuildingBlocks.Application.Exceptions.ValidationException ||
                ex is FluentValidation.ValidationException ||
                IsDuplicateKeyException(ex))
            {
                _logger.LogWarning(ex.Message);
            }
            else
            {
                _logger.LogError(ex, ex.Message);
            }

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
            FluentValidation.ValidationException
                => HttpStatusCode.BadRequest,

            HRMS.BuildingBlocks.Application.Exceptions.ValidationException
                => HttpStatusCode.BadRequest,

            NotFoundException
                => HttpStatusCode.NotFound,

            ConflictException
                => HttpStatusCode.Conflict,

            DbUpdateException when IsDuplicateKeyException(exception)
                => HttpStatusCode.Conflict,

            SqlException when IsDuplicateKeyException(exception)
                => HttpStatusCode.Conflict,

            UnauthorizedException
                => HttpStatusCode.Unauthorized,

            BusinessException
                => HttpStatusCode.BadRequest,

            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)statusCode;

        IDictionary<string, string[]>? errors = exception switch
        {
            FluentValidation.ValidationException validationException
                => validationException.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()),

            HRMS.BuildingBlocks.Application.Exceptions.ValidationException validationException
                => validationException.Errors,

            _ => null
        };

        var message = exception switch
        {
            FluentValidation.ValidationException
                => "Validation failed.",

            DbUpdateException when IsDuplicateKeyException(exception)
                => GetDuplicateKeyMessage(exception),

            SqlException when IsDuplicateKeyException(exception)
                => GetDuplicateKeyMessage(exception),

            _ when statusCode == HttpStatusCode.InternalServerError
                => "An unexpected error occurred.",

            _ => exception.Message
        };

        var response = new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = message,
            TraceId = context.TraceIdentifier,
            Errors = errors
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }

    private static bool IsDuplicateKeyException(Exception exception)
    {
        var sqlException = FindSqlException(exception);

        return sqlException is not null &&
               (sqlException.Number == 2601 ||
                sqlException.Number == 2627);
    }

    private static SqlException? FindSqlException(Exception exception)
    {
        var current = exception;

        while (current is not null)
        {
            if (current is SqlException sqlException)
                return sqlException;

            current = current.InnerException;
        }

        return null;
    }

    private static string GetDuplicateKeyMessage(Exception exception)
    {
        var sqlException = FindSqlException(exception);

        if (sqlException?.Message.Contains(
                "IX_Employees_Email",
                StringComparison.OrdinalIgnoreCase) == true)
        {
            return "An employee with this email already exists.";
        }

        return "A record with the same unique value already exists.";
    }
}
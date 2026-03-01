using System.Net;
using System.Text.Json;
using RealtimeDashboard.API.Models;
using RealtimeDashboard.Application.Common.Exceptions;

namespace RealtimeDashboard.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "Unhandled exception : {message}",ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, response) = exception switch
        {
            NotFoundException ex => (
                HttpStatusCode.NotFound,
                new ErrorResponse(
                    "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    "Resource Not Found",
                    404,
                    ex.Message
                )
            ),
            ValidationException ex => (
                HttpStatusCode.BadRequest,
                new ErrorResponse(
                    "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "Validation Failed",
                    400,
                    "One or more validation errors occurred.",
                    ex.Errors
                )
            ),
            ArgumentException ex => (
                HttpStatusCode.BadRequest,
                new ErrorResponse(
                    "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "Bad Request",
                    400,
                    ex.Message
                )
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                new ErrorResponse(
                    "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    "Internal Server Error",
                    500,
                    "An unexpected error occurred."
                )
            )
        };
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
using FluentValidation;
using System.Text.Json;

namespace PersonalHub.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
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
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400;

            var errors = ex.Errors
                .Select(x => x.ErrorMessage)
                .ToList();

            await context.Response.WriteAsJsonAsync(
                new
                {
                    Errors = errors
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = 500;

            await context.Response.WriteAsJsonAsync(
                new
                {
                    Message =
                        "An unexpected error occurred"
                });
        }
    }
}
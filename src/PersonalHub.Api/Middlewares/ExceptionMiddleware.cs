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
            _logger.LogWarning(
                "Validation error: {Errors}",
                string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));

            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";

            var response = new
            {
                errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray())
            };

            var options = new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            };

            await context.Response.WriteAsJsonAsync(response, options);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(
                new
                {
                    message = "An unexpected error occurred"
                });
        }
    }
}

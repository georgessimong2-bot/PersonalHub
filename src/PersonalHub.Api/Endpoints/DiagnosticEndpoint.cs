using System.Security.Claims;

namespace PersonalHub.Api.Endpoints;

public static class DiagnosticEndpoint
{
    public static void MapDiagnosticEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/diagnostic");

        // Public endpoint to check server status
        group.MapGet("/health", () =>
        {
            return Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        });

        // Protected endpoint to check authentication
        group.MapGet("/auth-info", (HttpContext context) =>
        {
            var claims = new Dictionary<string, object>();

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                claims["IsAuthenticated"] = true;
                claims["Name"] = context.User.Identity.Name ?? "Unknown";
                claims["Claims"] = context.User.Claims
                    .Select(c => new { Type = c.Type, Value = c.Value })
                    .ToList();
            }
            else
            {
                claims["IsAuthenticated"] = false;
                claims["Message"] = "User is not authenticated";
            }

            return Results.Ok(claims);
        })
        .RequireAuthorization()
        .WithName("GetAuthInfo")
        .WithOpenApi();
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace PersonalHub.Api.Endpoints;

public static class AuthCheckEndpoints
{
    public static void MapAuthCheckEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
            .WithName("AuthCheck")
            .WithOpenApi();

        group.MapGet("check-admin", CheckAdmin)
            .WithName("CheckAdmin")
            .WithOpenApi()
            .RequireAuthorization("AdminOnly");
    }

    [Authorize(Roles = "ADMIN")]
    private static async Task<IResult> CheckAdmin()
    {
        return Results.Ok(new { isAdmin = true });
    }
}

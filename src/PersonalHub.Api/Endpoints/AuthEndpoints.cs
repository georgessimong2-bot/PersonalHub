using MediatR;
using PersonalHub.Application.Features.Auth.Register;
using PersonalHub.Application.Features.Auth.Login;

namespace PersonalHub.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register",
            async (
                RegisterCommand command,
                IMediator mediator) =>
            {
                var result =
                    await mediator.Send(command);

                return Results.Ok(result);
            })
            .WithName("Register")
            .WithOpenApi()
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/login",
            async (
                LoginCommand command,
                IMediator mediator) =>
            {
                var result =
                    await mediator.Send(command);

                return Results.Ok(result);
            })
            .WithName("Login")
            .WithOpenApi()
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
    }
}
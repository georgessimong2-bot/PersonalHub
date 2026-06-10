using MediatR;
using PersonalHub.Application.Features.Account.UpdateProfile;
using PersonalHub.Application.Features.Users.Common;
using PersonalHub.Application.Features.Users.GetUserById;
using System.Security.Claims;

namespace PersonalHub.Api.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/account")
            .RequireAuthorization();

        group.MapGet("/profile", async (
            ClaimsPrincipal user,
            IMediator mediator) =>
        {
            var userId = user.FindFirst("sub")?.Value;

            if (userId is null)
                return Results.Unauthorized();

            var result = await mediator.Send(new GetUserByIdCommand(userId));

            return result is null
                ? Results.NotFound()
                : Results.Ok(result);
        });

        group.MapPut("/profile", async (
        UpdateProfileDto dto,
        ClaimsPrincipal user,
        HttpContext httpContext,
        IMediator mediator) =>
        {
            Console.WriteLine("===== CLAIMS =====");
            Console.WriteLine("Authenticated = " + user.Identity?.IsAuthenticated);
            Console.WriteLine("Name = " + user.Identity?.Name);
            Console.WriteLine("Claims count = " + user.Claims.Count());
            foreach (var c in user.Claims)
            {
                Console.WriteLine($"{c.Type} = {c.Value}");
            }

            Console.WriteLine("==================");

            var userId = user.FindFirst("sub")?.Value;

            Console.WriteLine("USER ID IN API = " + userId);

            if (userId is null)
                return Results.Unauthorized();

            await mediator.Send(new UpdateProfileCommand
            {
                UserId = userId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber
            });

            return Results.NoContent();
        });
    }
}
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
            IMediator mediator) =>
        {
            var userId = user.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Results.Unauthorized();

            try
            {
                await mediator.Send(new UpdateProfileCommand
                {
                    UserId = userId,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Address = dto.Address,
                    PhoneNumber = dto.PhoneNumber
                });

                return Results.NoContent();
            }
            catch (FluentValidation.ValidationException ex)
            {
                return Results.BadRequest(new
                {
                    Errors = ex.Errors.Select(e => e.ErrorMessage)
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    title: "Error updating profile",
                    detail: ex.Message,
                    statusCode: 500);
            }
        });
    }
}
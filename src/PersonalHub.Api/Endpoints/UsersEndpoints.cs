using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Users.Common;
using PersonalHub.Application.Features.Users.CreateUser;
using PersonalHub.Application.Features.Users.DeleteUser;
using PersonalHub.Application.Features.Users.GetUserById;
using PersonalHub.Application.Features.Users.UpdateUser;
using PersonalHub.Application.Features.Users.AssignRole;

namespace PersonalHub.Api.Endpoints;

public static class UsersEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users");

        group.MapGet("", async (IIdentityService service) =>
        {
            var users = await service.GetUsersAsync();

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                UserName = u.UserName,
                Role = u.Role
            });
        });

        //DELETE

        group.MapDelete("{id}",
            async (
                string id,
                IMediator mediator) =>
            {
                await mediator.Send(
                    new DeleteUserCommand(id));

                return Results.NoContent();
            });

        // GET BY ID

        group.MapGet("{id}",
            async (
                string id,
                IMediator mediator) =>
            {
                var user =
                    await mediator.Send(
                        new GetUserByIdCommand(id));

                return user is null
                    ? Results.NotFound()
                    : Results.Ok(user);
            });

        // CREATE

        group.MapPost("",
            async (
                CreateUserCommand command,
                IMediator mediator) =>
            {
                var result =
                    await mediator.Send(command);

                return Results.Ok(result);
            });

        // UPDATE

        group.MapPut("{id}",
            async (
                string id,
                UpdateUserCommand command,
                IMediator mediator) =>
            {
                command.Id = id;

                await mediator.Send(command);

                return Results.NoContent();
            });

        // ASSIGN ROLE

        group.MapPost("{id}/assign-role",
            async (
                string id,
                AssignRoleCommand command,
                IMediator mediator) =>
            {
                command.UserId = id;

                await mediator.Send(command);

                return Results.NoContent();
            });
    }
}
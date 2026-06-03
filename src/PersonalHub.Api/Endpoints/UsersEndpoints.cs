using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Users.Common;

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
                Email = u.Email!
            });
        });
    }
}
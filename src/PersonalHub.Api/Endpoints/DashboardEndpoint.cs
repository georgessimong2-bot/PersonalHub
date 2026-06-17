using MediatR;
using PersonalHub.Application.Features.Dashboard.GetDashboard;
namespace PersonalHub.Api.Endpoints;

public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dashboard");

        group.MapGet("/", async
            (IMediator mediator) =>
        {
            return Results.Ok(
                await mediator.Send(
                    new GetDashboardCommand()));
        });
    }
}
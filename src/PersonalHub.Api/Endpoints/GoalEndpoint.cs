using MediatR;
using PersonalHub.Application.Features.Goals.CreateGoal;
using PersonalHub.Application.Features.Goals.DeleteGoal;
using PersonalHub.Application.Features.Goals.GetGoalById;
using PersonalHub.Application.Features.Goals.GetGoals;
using PersonalHub.Application.Features.Goals.GetGoalStatistics;
using PersonalHub.Application.Features.Goals.IncrementGoal;
using PersonalHub.Application.Features.Goals.UpdateGoal;

namespace PersonalHub.Api.Endpoints;

public static class GoalEndpoints
{
    public static void MapGoalEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/goals")
            .RequireAuthorization();

        group.MapGet("/", async (
            IMediator mediator) =>
        {
            return await mediator.Send(
                new GetGoalsCommand());
        });

        group.MapPost("/", async (
            CreateGoalCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);

            return Results.Ok(id);
        });

        group.MapDelete("/{id}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(
                new DeleteGoalCommand(id));

            return Results.NoContent();
        });

        group.MapPatch("/{id}/increment",
            async (Guid id, IMediator mediator) =>
            {
                await mediator.Send(
                    new IncrementGoalCommand(id));

                return Results.NoContent();
            });

        group.MapGet("/statistics",
            async (IMediator mediator) =>
            {
                var result = await mediator.Send(
                    new GetGoalStatisticsCommand());

                return Results.Ok(result);
            });

        group.MapGet("/{id:guid}",
            async (
                Guid id,
                IMediator mediator) =>
            {
                var goal =
                    await mediator.Send(
                        new GetGoalByIdCommand(id));

                return goal is null
                    ? Results.NotFound()
                    : Results.Ok(goal);
            });

        group.MapPut("/{id:guid}",
            async (
                Guid id,
                UpdateGoalCommand command,
                IMediator mediator) =>
            {
                await mediator.Send(
                    command with { Id = id });

                return Results.NoContent();
            });
    }
}
using MediatR;
using PersonalHub.Application.Features.AI.GenerateGoalAdvice;

namespace PersonalHub.Api.Endpoints;

public static class AiEndpoints
{
    public static void MapAiEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/ai")
            .RequireAuthorization();

        group.MapPost(
            "/goals/{id:guid}/advice",
            async (
                Guid id,
                IMediator mediator) =>
            {
                Console.WriteLine("AI ENDPOINT CALLED");

                var result =
                    await mediator.Send(
                        new GenerateGoalAdviceCommand(id));

                Console.WriteLine("AI RESULT = " + result);

                return Results.Ok(result);
            });
    }
}
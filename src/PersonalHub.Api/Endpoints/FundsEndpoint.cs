using MediatR;
using PersonalHub.Application.Features.Funds.CreateFund;
using PersonalHub.Application.Features.Funds.DeleteFund;
using PersonalHub.Application.Features.Funds.GetFundById;
using PersonalHub.Application.Features.Funds.GetFunds;
using PersonalHub.Application.Features.Funds.UpdateFund;

namespace PersonalHub.Api.Endpoints;

public static class FundEndpoints
{
    public static void MapFundEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/funds");

        // GET ALL
        group.MapGet("/", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetFundsCommand());
        });

        // GET BY ID 
        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetFundByIdCommand(id));

            return result is null
                ? Results.NotFound()
                : Results.Ok(result);
        });

        // CREATE
        group.MapPost("/", async (CreateFundCommand command, IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Ok(id);
        });

        // UPDATE
        group.MapPut("/{id:guid}", async (Guid id, UpdateFundCommand command, IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        // DELETE
        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            await mediator.Send(new DeleteFundCommand(id));
            return Results.NoContent();
        });
    }
}
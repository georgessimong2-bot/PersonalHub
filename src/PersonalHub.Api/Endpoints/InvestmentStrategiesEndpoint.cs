using MediatR;
using PersonalHub.Application.Features.InvestmentStrategies.CreateInvestmentStrategy;
using PersonalHub.Application.Features.InvestmentStrategies.DeleteInvestmentStrategy;
using PersonalHub.Application.Features.InvestmentStrategies.GetInvestmentStrategyById;
using PersonalHub.Application.Features.InvestmentStrategies.GetInvestmentStrategies;
using PersonalHub.Application.Features.InvestmentStrategies.UpdateInvestmentStrategy;

namespace PersonalHub.Api.Endpoints;

public static class InvestmentStrategiesEndpoint
{
    public static void MapInvestmentStrategiesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/investment-strategies")
            .WithTags("Investment Strategies");

        group.MapGet("/", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetInvestmentStrategiesQuery());
        });

        group.MapPost("/", async (
            CreateInvestmentStrategyCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/investment-strategies/{id}", id);
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            var investmentStrategy = await mediator.Send(
                new GetInvestmentStrategyByIdQuery(id));

            return investmentStrategy is null
                ? Results.NotFound()
                : Results.Ok(investmentStrategy);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateInvestmentStrategyCommand command,
            IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(new DeleteInvestmentStrategyCommand(id));
            return Results.NoContent();
        });
    }
}

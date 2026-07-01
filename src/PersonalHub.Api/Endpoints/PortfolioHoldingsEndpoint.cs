using MediatR;
using PersonalHub.Application.Features.PortfolioHoldings.CreatePortfolioHolding;
using PersonalHub.Application.Features.PortfolioHoldings.DeletePortfolioHolding;
using PersonalHub.Application.Features.PortfolioHoldings.GetPortfolioHoldingById;
using PersonalHub.Application.Features.PortfolioHoldings.GetPortfolioHoldingsByPortfolioId;
using PersonalHub.Application.Features.PortfolioHoldings.UpdatePortfolioHolding;

namespace PersonalHub.Api.Endpoints;

public static class PortfolioHoldingsEndpoint
{
    public static void MapPortfolioHoldingsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/portfolio-holdings")
            .WithTags("Portfolio Holdings");

        group.MapGet("/", async (
            Guid portfolioId,
            IMediator mediator) =>
        {
            return await mediator.Send(
                new GetPortfolioHoldingsByPortfolioIdQuery(portfolioId));
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            var holding = await mediator.Send(
                new GetPortfolioHoldingByIdQuery(id));

            return holding is null
                ? Results.NotFound()
                : Results.Ok(holding);
        });

        group.MapPost("/", async (
            CreatePortfolioHoldingCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/portfolio-holdings/{id}", id);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdatePortfolioHoldingCommand command,
            IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(new DeletePortfolioHoldingCommand(id));
            return Results.NoContent();
        });
    }
}

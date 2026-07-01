using MediatR;
using PersonalHub.Application.Features.Portfolios.CreatePortfolio;
using PersonalHub.Application.Features.Portfolios.DeletePortfolio;
using PersonalHub.Application.Features.Portfolios.GetPortfolioById;
using PersonalHub.Application.Features.Portfolios.GetPortfolios;
using PersonalHub.Application.Features.Portfolios.UpdatePortfolio;

namespace PersonalHub.Api.Endpoints;

public static class PortfoliosEndpoint
{
    public static void MapPortfoliosEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/portfolios")
            .WithTags("Portfolios");

        group.MapGet("/", async (
            Guid? shareClassId,
            IMediator mediator) =>
        {
            return await mediator.Send(new GetPortfoliosQuery(shareClassId));
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            var portfolio = await mediator.Send(
                new GetPortfolioByIdQuery(id));

            return portfolio is null
                ? Results.NotFound()
                : Results.Ok(portfolio);
        });

        group.MapPost("/", async (
            CreatePortfolioCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/portfolios/{id}", id);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdatePortfolioCommand command,
            IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(new DeletePortfolioCommand(id));
            return Results.NoContent();
        });
    }
}

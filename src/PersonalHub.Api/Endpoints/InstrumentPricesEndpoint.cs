using MediatR;
using PersonalHub.Application.Features.InstrumentPrices.CreateInstrumentPrice;
using PersonalHub.Application.Features.InstrumentPrices.DeleteInstrumentPrice;
using PersonalHub.Application.Features.InstrumentPrices.GetInstrumentPricesByInstrumentId;
using PersonalHub.Application.Features.InstrumentPrices.UpdateInstrumentPrice;

namespace PersonalHub.Api.Endpoints;

public static class InstrumentPricesEndpoint
{
    public static void MapInstrumentPricesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/instrument-prices")
            .WithTags("Instrument Prices");

        group.MapGet("/", async (
            Guid instrumentId,
            IMediator mediator) =>
        {
            return await mediator.Send(
                new GetInstrumentPricesByInstrumentIdQuery(instrumentId));
        });

        group.MapPost("/", async (
            CreateInstrumentPriceCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/instrument-prices/{id}", id);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateInstrumentPriceCommand command,
            IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(new DeleteInstrumentPriceCommand(id));
            return Results.NoContent();
        });
    }
}

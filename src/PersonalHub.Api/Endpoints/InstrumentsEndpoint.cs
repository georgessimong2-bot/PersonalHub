using MediatR;
using PersonalHub.Application.Features.Instruments.CreateInstrument;
using PersonalHub.Application.Features.Instruments.DeleteInstrument;
using PersonalHub.Application.Features.Instruments.GetInstrumentById;
using PersonalHub.Application.Features.Instruments.GetInstruments;
using PersonalHub.Application.Features.Instruments.UpdateInstrument;

namespace PersonalHub.Api.Endpoints;

public static class InstrumentsEndpoint
{
    public static void MapInstrumentsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/instruments")
            .WithTags("Instruments");

        group.MapGet("/", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetInstrumentsQuery());
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            var instrument = await mediator.Send(
                new GetInstrumentByIdQuery(id));

            return instrument is null
                ? Results.NotFound()
                : Results.Ok(instrument);
        });

        group.MapPost("/", async (
            CreateInstrumentCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/instruments/{id}", id);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateInstrumentCommand command,
            IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(new DeleteInstrumentCommand(id));
            return Results.NoContent();
        });
    }
}

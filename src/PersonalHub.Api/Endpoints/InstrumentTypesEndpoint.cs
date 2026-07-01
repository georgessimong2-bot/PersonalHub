using MediatR;
using PersonalHub.Application.Features.InstrumentTypes.CreateInstrumentType;
using PersonalHub.Application.Features.InstrumentTypes.DeleteInstrumentType;
using PersonalHub.Application.Features.InstrumentTypes.GetInstrumentTypeById;
using PersonalHub.Application.Features.InstrumentTypes.GetInstrumentTypes;
using PersonalHub.Application.Features.InstrumentTypes.UpdateInstrumentType;

namespace PersonalHub.Api.Endpoints;

public static class InstrumentTypesEndpoint
{
    public static void MapInstrumentTypesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/instrument-types")
            .WithTags("Instrument Types");

        group.MapGet("/", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetInstrumentTypesQuery());
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            var instrumentType = await mediator.Send(
                new GetInstrumentTypeByIdQuery(id));

            return instrumentType is null
                ? Results.NotFound()
                : Results.Ok(instrumentType);
        });

        group.MapPost("/", async (
            CreateInstrumentTypeCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/instrument-types/{id}", id);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateInstrumentTypeCommand command,
            IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(new DeleteInstrumentTypeCommand(id));
            return Results.NoContent();
        });
    }
}

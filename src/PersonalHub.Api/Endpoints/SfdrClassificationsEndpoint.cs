using MediatR;
using PersonalHub.Application.Features.SfdrClassifications.CreateSfdrClassification;
using PersonalHub.Application.Features.SfdrClassifications.DeleteSfdrClassification;
using PersonalHub.Application.Features.SfdrClassifications.GetSfdrClassificationById;
using PersonalHub.Application.Features.SfdrClassifications.GetSfdrClassifications;
using PersonalHub.Application.Features.SfdrClassifications.UpdateSfdrClassification;

namespace PersonalHub.Api.Endpoints;

public static class SfdrClassificationsEndpoint
{
    public static void MapSfdrClassificationsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sfdr-classifications")
            .WithTags("SFDR Classifications");

        group.MapGet("/", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetSfdrClassificationsQuery());
        });

        group.MapPost("/", async (
            CreateSfdrClassificationCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/sfdr-classifications/{id}", id);
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            var sfdrClassification = await mediator.Send(
                new GetSfdrClassificationByIdQuery(id));

            return sfdrClassification is null
                ? Results.NotFound()
                : Results.Ok(sfdrClassification);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateSfdrClassificationCommand command,
            IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(new DeleteSfdrClassificationCommand(id));
            return Results.NoContent();
        });
    }
}

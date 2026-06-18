using MediatR;
using PersonalHub.Application.Features.ShareClasses.CreateShareClass;
using PersonalHub.Application.Features.ShareClasses.DeleteShareClass;
using PersonalHub.Application.Features.ShareClasses.GetShareClassById;
using PersonalHub.Application.Features.ShareClasses.GetShareClasses;
using PersonalHub.Application.Features.ShareClasses.UpdateShareClass;

namespace PersonalHub.Api.Endpoints;

public static class ShareClassesEndpoint
{
    public static void MapShareClassesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/share-classes")
            .WithTags("Share Classes");

        group.MapGet("/", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetShareClassesQuery());
        });

        group.MapPost("/", async (
            CreateShareClassCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/share-classes/{id}", id);
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            var shareClass = await mediator.Send(
                new GetShareClassByIdQuery(id));

            return shareClass is null
                ? Results.NotFound()
                : Results.Ok(shareClass);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateShareClassCommand command,
            IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(new DeleteShareClassCommand(id));
            return Results.NoContent();
        });
    }
}

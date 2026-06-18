using MediatR;
using PersonalHub.Application.Features.AssetClasses.CreateAssetClass;
using PersonalHub.Application.Features.AssetClasses.DeleteAssetClass;
using PersonalHub.Application.Features.AssetClasses.GetAssetClassById;
using PersonalHub.Application.Features.AssetClasses.GetAssetClasses;
using PersonalHub.Application.Features.AssetClasses.UpdateAssetClass;

namespace PersonalHub.Api.Endpoints;

public static class AssetClassesEndpoint
{
    public static void MapAssetClassesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/asset-classes")
            .WithTags("Asset Classes");

        group.MapGet("/", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetAssetClassesQuery());
        });

        group.MapPost("/", async (
            CreateAssetClassCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/asset-classes/{id}", id);
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            var assetClass = await mediator.Send(
                new GetAssetClassByIdQuery(id));

            return assetClass is null
                ? Results.NotFound()
                : Results.Ok(assetClass);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateAssetClassCommand command,
            IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(new DeleteAssetClassCommand(id));
            return Results.NoContent();
        });
    }
}

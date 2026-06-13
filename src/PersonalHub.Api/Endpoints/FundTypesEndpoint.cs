using MediatR;
using PersonalHub.Application.Features.FundTypes.CreateFundType;
using PersonalHub.Application.Features.FundTypes.DeleteFundType;
using PersonalHub.Application.Features.FundTypes.GetFundTypeById;
using PersonalHub.Application.Features.FundTypes.GetFundTypes;
using PersonalHub.Application.Features.FundTypes.UpdateFundType;

namespace PersonalHub.Api.Endpoints;

public static class FundTypeEndpoints
{
    public static void MapFundTypeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/fundtypes");

        group.MapPost("/", async (CreateFundTypeCommand command, IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Ok(id);
        });

        group.MapGet("/", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetFundTypesCommand());
        });

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            return await mediator.Send(new GetFundTypeByIdCommand(id));
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateFundTypeCommand command, IMediator mediator) =>
        {
            if (id != command.Id)
                return Results.BadRequest();

            await mediator.Send(command);
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            await mediator.Send(new DeleteFundTypeCommand(id));
            return Results.NoContent();
        });
    }
}
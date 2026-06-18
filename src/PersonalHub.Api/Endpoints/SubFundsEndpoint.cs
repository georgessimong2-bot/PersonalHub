using MediatR;
using PersonalHub.Application.Features.SubFunds.CreateSubFund;
using PersonalHub.Application.Features.SubFunds.DeleteSubFund;
using PersonalHub.Application.Features.SubFunds.GetSubFundById;
using PersonalHub.Application.Features.SubFunds.GetSubFunds;
using PersonalHub.Application.Features.SubFunds.UpdateSubFund;

namespace PersonalHub.Api.Endpoints;

public static class SubFundsEndpoint
{
    public static void MapSubFundsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sub-funds")
            .WithTags("Sub Funds");

        group.MapGet("/", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetSubFundsQuery());
        });

        group.MapPost("/", async (
            CreateSubFundCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/sub-funds/{id}", id);
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            var subFund = await mediator.Send(
                new GetSubFundByIdQuery(id));

            return subFund is null
                ? Results.NotFound()
                : Results.Ok(subFund);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateSubFundCommand command,
            IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(new DeleteSubFundCommand(id));
            return Results.NoContent();
        });
    }
}

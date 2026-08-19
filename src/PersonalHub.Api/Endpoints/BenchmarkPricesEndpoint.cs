using MediatR;
using PersonalHub.Application.Features.BenchmarkPrices.CreateBenchmarkPrice;
using PersonalHub.Application.Features.BenchmarkPrices.DeleteBenchmarkPrice;
using PersonalHub.Application.Features.BenchmarkPrices.GetBenchmarkPricesByBenchmarkId;
using PersonalHub.Application.Features.BenchmarkPrices.UpdateBenchmarkPrice;

namespace PersonalHub.Api.Endpoints;

public static class BenchmarkPricesEndpoint
{
    public static void MapBenchmarkPricesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/benchmark-prices")
            .WithTags("Benchmark Prices");

        group.MapGet("/", async (
            Guid benchmarkId,
            IMediator mediator) =>
        {
            return await mediator.Send(
                new GetBenchmarkPricesByBenchmarkIdQuery(benchmarkId));
        });

        group.MapPost("/", async (
            CreateBenchmarkPriceCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/benchmark-prices/{id}", id);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateBenchmarkPriceCommand command,
            IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(new DeleteBenchmarkPriceCommand(id));
            return Results.NoContent();
        });
    }
}

using MediatR;
using PersonalHub.Application.Features.Benchmarks.CreateBenchmark;
using PersonalHub.Application.Features.Benchmarks.DeleteBenchmark;
using PersonalHub.Application.Features.Benchmarks.GetBenchmarkById;
using PersonalHub.Application.Features.Benchmarks.GetBenchmarks;
using PersonalHub.Application.Features.Benchmarks.UpdateBenchmark;

namespace PersonalHub.Api.Endpoints;

public static class BenchmarksEndpoint
{
    public static void MapBenchmarksEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/benchmarks")
            .WithTags("Benchmarks");

        group.MapGet("/", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetBenchmarksQuery());
        });

        group.MapPost("/", async (
            CreateBenchmarkCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/benchmarks/{id}", id);
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            var benchmark = await mediator.Send(
                new GetBenchmarkByIdQuery(id));

            return benchmark is null
                ? Results.NotFound()
                : Results.Ok(benchmark);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateBenchmarkCommand command,
            IMediator mediator) =>
        {
            await mediator.Send(command with { Id = id });
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(new DeleteBenchmarkCommand(id));
            return Results.NoContent();
        });
    }
}

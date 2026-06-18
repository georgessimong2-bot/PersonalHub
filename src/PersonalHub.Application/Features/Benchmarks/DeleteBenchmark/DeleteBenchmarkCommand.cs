using MediatR;

namespace PersonalHub.Application.Features.Benchmarks.DeleteBenchmark;

public record DeleteBenchmarkCommand(Guid Id)
    : IRequest;

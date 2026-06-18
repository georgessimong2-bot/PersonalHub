using MediatR;

namespace PersonalHub.Application.Features.Benchmarks.UpdateBenchmark;

public record UpdateBenchmarkCommand(
    Guid Id,
    string Name,
    string? BloombergTicker,
    string? ReutersCode,
    string? Provider,
    string? Description,
    bool IsActive)
    : IRequest;

using MediatR;

namespace PersonalHub.Application.Features.Benchmarks.UpdateBenchmark;

public record UpdateBenchmarkCommand(
    Guid Id,
    string Name,
    string? BloombergTicker,
    string? ReutersCode,
    string? Provider,
    Guid? CurrencyId,
    string? Description,
    bool IsActive)
    : IRequest;


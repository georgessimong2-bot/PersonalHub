using MediatR;

namespace PersonalHub.Application.Features.Benchmarks.CreateBenchmark;

public class CreateBenchmarkCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string? BloombergTicker { get; set; }

    public string? ReutersCode { get; set; }

    public string? Provider { get; set; }

    public Guid? CurrencyId { get; set; }

    public string? Description { get; set; }
}


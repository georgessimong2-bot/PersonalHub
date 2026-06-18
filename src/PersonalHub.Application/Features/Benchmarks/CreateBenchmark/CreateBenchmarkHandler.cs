using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.Benchmarks.CreateBenchmark;

public class CreateBenchmarkHandler
    : IRequestHandler<CreateBenchmarkCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateBenchmarkHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateBenchmarkCommand request,
        CancellationToken cancellationToken)
    {
        var benchmark = new Benchmark
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            BloombergTicker = request.BloombergTicker,
            ReutersCode = request.ReutersCode,
            Provider = request.Provider,
            Description = request.Description,
            IsActive = true
        };

        _context.Benchmarks.Add(benchmark);
        await _context.SaveChangesAsync(cancellationToken);

        return benchmark.Id;
    }
}

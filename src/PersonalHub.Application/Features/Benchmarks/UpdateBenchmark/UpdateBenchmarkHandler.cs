using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Benchmarks.UpdateBenchmark;

public class UpdateBenchmarkHandler
    : IRequestHandler<UpdateBenchmarkCommand>
{
    private readonly IAppDbContext _context;

    public UpdateBenchmarkHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateBenchmarkCommand request,
        CancellationToken cancellationToken)
    {
        var benchmark = await _context.Benchmarks
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (benchmark is null)
            throw new Exception("Benchmark not found");

        benchmark.Name = request.Name;
        benchmark.BloombergTicker = request.BloombergTicker;
        benchmark.ReutersCode = request.ReutersCode;
        benchmark.Provider = request.Provider;
        benchmark.Description = request.Description;
        benchmark.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

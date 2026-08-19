using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.BenchmarkPrices.DeleteBenchmarkPrice;

public class DeleteBenchmarkPriceHandler
    : IRequestHandler<DeleteBenchmarkPriceCommand>
{
    private readonly IAppDbContext _context;

    public DeleteBenchmarkPriceHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteBenchmarkPriceCommand request,
        CancellationToken cancellationToken)
    {
        var benchmarkPrice = await _context.BenchmarkPrices
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new Exception("Benchmark price not found");

        _context.BenchmarkPrices.Remove(benchmarkPrice);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

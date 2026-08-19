using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Exceptions;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.BenchmarkPrices.UpdateBenchmarkPrice;

public class UpdateBenchmarkPriceHandler
    : IRequestHandler<UpdateBenchmarkPriceCommand>
{
    private readonly IAppDbContext _context;

    public UpdateBenchmarkPriceHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateBenchmarkPriceCommand request,
        CancellationToken cancellationToken)
    {
        var normalizedPriceDate = request.PriceDate.Date;

        var benchmarkPrice = await _context.BenchmarkPrices
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new BusinessException("Benchmark price not found");

        var conflictingPrice = await _context.BenchmarkPrices
            .AnyAsync(
                x =>
                    x.Id != request.Id &&
                    x.BenchmarkId == benchmarkPrice.BenchmarkId &&
                    x.PriceDate.Date == normalizedPriceDate,
                cancellationToken);

        if (conflictingPrice)
            throw new BusinessException("A price already exists for this benchmark on the selected date");

        benchmarkPrice.Price = request.Price;
        benchmarkPrice.PriceDate = normalizedPriceDate;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

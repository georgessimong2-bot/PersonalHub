using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.BenchmarkPrices.CreateBenchmarkPrice;

public class CreateBenchmarkPriceHandler
    : IRequestHandler<CreateBenchmarkPriceCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateBenchmarkPriceHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateBenchmarkPriceCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await _context.BenchmarkPrices
            .FirstOrDefaultAsync(
                x => x.BenchmarkId == request.BenchmarkId && x.PriceDate == request.PriceDate,
                cancellationToken);

        if (existing is not null)
        {
            existing.Price = request.Price;
            await _context.SaveChangesAsync(cancellationToken);
            return existing.Id;
        }

        var benchmarkPrice = new BenchmarkPrice
        {
            Id = Guid.NewGuid(),
            BenchmarkId = request.BenchmarkId,
            Price = request.Price,
            PriceDate = request.PriceDate
        };

        _context.BenchmarkPrices.Add(benchmarkPrice);

        await _context.SaveChangesAsync(cancellationToken);

        return benchmarkPrice.Id;
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.BenchmarkPrices.Common;

namespace PersonalHub.Application.Features.BenchmarkPrices.GetBenchmarkPricesByBenchmarkId;

public class GetBenchmarkPricesByBenchmarkIdHandler
    : IRequestHandler<GetBenchmarkPricesByBenchmarkIdQuery, List<BenchmarkPriceDto>>
{
    private readonly IAppDbContext _context;

    public GetBenchmarkPricesByBenchmarkIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BenchmarkPriceDto>> Handle(
        GetBenchmarkPricesByBenchmarkIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.BenchmarkPrices
            .Where(x => x.BenchmarkId == request.BenchmarkId)
            .OrderByDescending(x => x.PriceDate)
            .Select(x => new BenchmarkPriceDto
            {
                Id = x.Id,
                BenchmarkId = x.BenchmarkId,
                Price = x.Price,
                PriceDate = x.PriceDate.Date
            })
            .ToListAsync(cancellationToken);
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Benchmarks.Common;

namespace PersonalHub.Application.Features.Benchmarks.GetBenchmarkById;

public class GetBenchmarkByIdHandler
    : IRequestHandler<GetBenchmarkByIdQuery, BenchmarkDto?>
{
    private readonly IAppDbContext _context;

    public GetBenchmarkByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<BenchmarkDto?> Handle(
        GetBenchmarkByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Benchmarks
            .Where(x => x.Id == request.Id)
            .Select(x => new BenchmarkDto
            {
                Id = x.Id,
                Name = x.Name,
                BloombergTicker = x.BloombergTicker,
                ReutersCode = x.ReutersCode,
                Provider = x.Provider,
                Description = x.Description,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}

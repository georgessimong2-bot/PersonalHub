using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Benchmarks.DeleteBenchmark;

public class DeleteBenchmarkHandler
    : IRequestHandler<DeleteBenchmarkCommand>
{
    private readonly IAppDbContext _context;

    public DeleteBenchmarkHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteBenchmarkCommand request,
        CancellationToken cancellationToken)
    {
        var benchmark = await _context.Benchmarks
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (benchmark is null)
            throw new Exception("Benchmark not found");

        _context.Benchmarks.Remove(benchmark);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

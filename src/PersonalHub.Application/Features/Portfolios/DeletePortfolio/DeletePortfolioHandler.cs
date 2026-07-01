using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Portfolios.DeletePortfolio;

public class DeletePortfolioHandler
    : IRequestHandler<DeletePortfolioCommand>
{
    private readonly IAppDbContext _context;

    public DeletePortfolioHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeletePortfolioCommand request,
        CancellationToken cancellationToken)
    {
        var portfolio = await _context.Portfolios
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new Exception("Portfolio not found");

        _context.Portfolios.Remove(portfolio);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

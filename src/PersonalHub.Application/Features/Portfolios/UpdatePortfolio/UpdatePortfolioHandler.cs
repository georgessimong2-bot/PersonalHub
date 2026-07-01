using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Portfolios.UpdatePortfolio;

public class UpdatePortfolioHandler
    : IRequestHandler<UpdatePortfolioCommand>
{
    private readonly IAppDbContext _context;

    public UpdatePortfolioHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdatePortfolioCommand request,
        CancellationToken cancellationToken)
    {
        var portfolio = await _context.Portfolios
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new Exception("Portfolio not found");

        portfolio.ShareClassId = request.ShareClassId;
        portfolio.ValuationDate = request.ValuationDate;
        portfolio.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

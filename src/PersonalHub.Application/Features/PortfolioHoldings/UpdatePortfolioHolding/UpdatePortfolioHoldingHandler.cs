using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.PortfolioHoldings.UpdatePortfolioHolding;

public class UpdatePortfolioHoldingHandler
    : IRequestHandler<UpdatePortfolioHoldingCommand>
{
    private readonly IAppDbContext _context;

    public UpdatePortfolioHoldingHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdatePortfolioHoldingCommand request,
        CancellationToken cancellationToken)
    {
        var holding = await _context.PortfolioHoldings
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new Exception("Portfolio holding not found");

        var portfolio = await _context.Portfolios
            .FirstOrDefaultAsync(x => x.Id == holding.PortfolioId, cancellationToken)
            ?? throw new Exception("Portfolio not found");

        var instrumentExists = await _context.Instruments
            .AnyAsync(x => x.Id == request.InstrumentId, cancellationToken);

        if (!instrumentExists)
            throw new Exception("Instrument not found");

        var priceAsOfValuationDate = await _context.InstrumentPrices
            .Where(x => x.InstrumentId == request.InstrumentId && x.PriceDate <= portfolio.ValuationDate)
            .OrderByDescending(x => x.PriceDate)
            .Select(x => (decimal?)x.Price)
            .FirstOrDefaultAsync(cancellationToken);

        holding.InstrumentId = request.InstrumentId;
        holding.Quantity = request.Quantity;
        holding.AverageCost = request.AverageCost;
        holding.MarketValue = priceAsOfValuationDate.HasValue
            ? request.Quantity * priceAsOfValuationDate.Value
            : null;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.PortfolioHoldings.CreatePortfolioHolding;

public class CreatePortfolioHoldingHandler
    : IRequestHandler<CreatePortfolioHoldingCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreatePortfolioHoldingHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreatePortfolioHoldingCommand request,
        CancellationToken cancellationToken)
    {
        var portfolio = await _context.Portfolios
            .FirstOrDefaultAsync(x => x.Id == request.PortfolioId, cancellationToken)
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

        var holding = new PortfolioHolding
        {
            Id = Guid.NewGuid(),
            PortfolioId = request.PortfolioId,
            InstrumentId = request.InstrumentId,
            Quantity = request.Quantity,
            AverageCost = request.AverageCost,
            MarketValue = priceAsOfValuationDate.HasValue
                ? request.Quantity * priceAsOfValuationDate.Value
                : null
        };

        _context.PortfolioHoldings.Add(holding);

        await _context.SaveChangesAsync(cancellationToken);

        return holding.Id;
    }
}

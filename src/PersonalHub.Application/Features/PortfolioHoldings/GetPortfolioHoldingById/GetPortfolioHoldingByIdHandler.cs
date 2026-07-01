using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Portfolios.Common;

namespace PersonalHub.Application.Features.PortfolioHoldings.GetPortfolioHoldingById;

public class GetPortfolioHoldingByIdHandler
    : IRequestHandler<GetPortfolioHoldingByIdQuery, PortfolioHoldingDto?>
{
    private readonly IAppDbContext _context;

    public GetPortfolioHoldingByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PortfolioHoldingDto?> Handle(
        GetPortfolioHoldingByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.PortfolioHoldings
            .Where(x => x.Id == request.Id)
            .Select(x => new PortfolioHoldingDto
            {
                Id = x.Id,
                PortfolioId = x.PortfolioId,
                InstrumentId = x.InstrumentId,
                InstrumentName = x.Instrument.Name,
                InstrumentISIN = x.Instrument.ISIN,
                Quantity = x.Quantity,
                AverageCost = x.AverageCost,
                MarketValue = x.MarketValue
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}

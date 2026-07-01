using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Portfolios.Common;

namespace PersonalHub.Application.Features.PortfolioHoldings.GetPortfolioHoldingsByPortfolioId;

public class GetPortfolioHoldingsByPortfolioIdHandler
    : IRequestHandler<GetPortfolioHoldingsByPortfolioIdQuery, List<PortfolioHoldingDto>>
{
    private readonly IAppDbContext _context;

    public GetPortfolioHoldingsByPortfolioIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PortfolioHoldingDto>> Handle(
        GetPortfolioHoldingsByPortfolioIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.PortfolioHoldings
            .Where(x => x.PortfolioId == request.PortfolioId)
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
            .ToListAsync(cancellationToken);
    }
}

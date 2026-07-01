using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Portfolios.Common;

namespace PersonalHub.Application.Features.Portfolios.GetPortfolioById;

public class GetPortfolioByIdHandler
    : IRequestHandler<GetPortfolioByIdQuery, PortfolioDto?>
{
    private readonly IAppDbContext _context;

    public GetPortfolioByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PortfolioDto?> Handle(
        GetPortfolioByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Portfolios
            .Where(x => x.Id == request.Id)
            .Select(x => new PortfolioDto
            {
                Id = x.Id,
                ShareClassId = x.ShareClassId,
                ShareClassName = x.ShareClass.Name,
                SubFundName = x.ShareClass.SubFund.Name,
                FundName = x.ShareClass.SubFund.Fund.Name,
                ValuationDate = x.ValuationDate,
                IsActive = x.IsActive,
                HoldingsCount = x.Holdings.Count,
                TotalMarketValue = x.Holdings.Sum(h => h.MarketValue),
                Holdings = x.Holdings.Select(h => new PortfolioHoldingDto
                {
                    Id = h.Id,
                    PortfolioId = h.PortfolioId,
                    InstrumentId = h.InstrumentId,
                    InstrumentName = h.Instrument.Name,
                    InstrumentISIN = h.Instrument.ISIN,
                    Quantity = h.Quantity,
                    AverageCost = h.AverageCost,
                    MarketValue = h.MarketValue
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}

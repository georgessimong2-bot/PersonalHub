using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Portfolios.Common;

namespace PersonalHub.Application.Features.Portfolios.GetPortfolios;

public class GetPortfoliosHandler
    : IRequestHandler<GetPortfoliosQuery, List<PortfolioDto>>
{
    private readonly IAppDbContext _context;

    public GetPortfoliosHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PortfolioDto>> Handle(
        GetPortfoliosQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Portfolios.AsQueryable();

        if (request.ShareClassId.HasValue)
        {
            query = query.Where(x => x.ShareClassId == request.ShareClassId.Value);
        }

        return await query
            .OrderByDescending(x => x.ValuationDate)
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
                TotalMarketValue = x.Holdings.Sum(h => h.MarketValue)
            })
            .ToListAsync(cancellationToken);
    }
}

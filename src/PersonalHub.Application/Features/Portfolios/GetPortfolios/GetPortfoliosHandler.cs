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
        var query = _context.Portfolios
            .Include(x => x.ShareClass)
                .ThenInclude(sc => sc.SubFund)
                    .ThenInclude(sf => sf.Currency)
            .Include(x => x.ShareClass.SubFund.Fund)
            .Include(x => x.Holdings)
                .ThenInclude(h => h.Instrument)
                    .ThenInclude(i => i.Currency)
            .AsQueryable();

        if (request.ShareClassId.HasValue)
        {
            query = query.Where(x => x.ShareClassId == request.ShareClassId.Value);
        }

        var portfolios = await query
            .OrderByDescending(x => x.ValuationDate)
            .ToListAsync(cancellationToken);

        var result = new List<PortfolioDto>();

        foreach (var portfolio in portfolios)
        {
            var subFundCurrencyId = portfolio.ShareClass.SubFund.CurrencyId;
            var portfolioDto = new PortfolioDto
            {
                Id = portfolio.Id,
                ShareClassId = portfolio.ShareClassId,
                ShareClassName = portfolio.ShareClass.Name,
                SubFundName = portfolio.ShareClass.SubFund.Name,
                FundName = portfolio.ShareClass.SubFund.Fund.Name,
                ValuationDate = portfolio.ValuationDate,
                IsActive = portfolio.IsActive,
                HoldingsCount = portfolio.Holdings.Count,
                TotalMarketValue = portfolio.Holdings.Sum(h => h.MarketValue),
                SubFundCurrencyCode = portfolio.ShareClass.SubFund.Currency?.Code,
                Holdings = new List<PortfolioHoldingDto>()
            };

            decimal? totalConvertedValue = 0;

            foreach (var holding in portfolio.Holdings)
            {
                var holdingDto = new PortfolioHoldingDto
                {
                    Id = holding.Id,
                    PortfolioId = holding.PortfolioId,
                    InstrumentId = holding.InstrumentId,
                    InstrumentName = holding.Instrument.Name,
                    InstrumentISIN = holding.Instrument.ISIN,
                    Quantity = holding.Quantity,
                    AverageCost = holding.AverageCost,
                    MarketValue = holding.MarketValue,
                    InstrumentCurrencyCode = holding.Instrument.Currency?.Code
                };

                if (holding.MarketValue.HasValue && subFundCurrencyId.HasValue)
                {
                    if (holding.Instrument.CurrencyId == subFundCurrencyId)
                    {
                        holdingDto.MarketValueInSubFundCurrency = holding.MarketValue;
                        holdingDto.HasExchangeRate = true;
                        totalConvertedValue += holding.MarketValue;
                    }
                    else
                    {
                        var exchangeRate = await _context.ExchangeRates
                            .Where(x =>
                                x.FromCurrencyId == holding.Instrument.CurrencyId &&
                                x.ToCurrencyId == subFundCurrencyId.Value &&
                                x.EffectiveDate <= portfolio.ValuationDate)
                            .OrderByDescending(x => x.EffectiveDate)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (exchangeRate != null)
                        {
                            holdingDto.MarketValueInSubFundCurrency = holding.MarketValue * exchangeRate.Rate;
                            holdingDto.HasExchangeRate = true;
                            totalConvertedValue += holdingDto.MarketValueInSubFundCurrency;
                        }
                        else
                        {
                            holdingDto.MarketValueInSubFundCurrency = holding.MarketValue;
                            holdingDto.HasExchangeRate = false;
                            totalConvertedValue += holding.MarketValue;
                        }
                    }
                }
                else
                {
                    holdingDto.MarketValueInSubFundCurrency = holding.MarketValue;
                    holdingDto.HasExchangeRate = holding.MarketValue.HasValue;
                    if (holding.MarketValue.HasValue)
                    {
                        totalConvertedValue += holding.MarketValue;
                    }
                }

                portfolioDto.Holdings.Add(holdingDto);
            }

            portfolioDto.TotalMarketValueInSubFundCurrency = totalConvertedValue;
            result.Add(portfolioDto);
        }

        return result;
    }
}

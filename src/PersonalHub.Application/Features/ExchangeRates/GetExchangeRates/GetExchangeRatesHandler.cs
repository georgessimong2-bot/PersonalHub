using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.ExchangeRates.Common;

namespace PersonalHub.Application.Features.ExchangeRates.GetExchangeRates;

public class GetExchangeRatesHandler
    : IRequestHandler<GetExchangeRatesQuery, List<ExchangeRateDto>>
{
    private readonly IAppDbContext _context;

    public GetExchangeRatesHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExchangeRateDto>> Handle(
        GetExchangeRatesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.ExchangeRates
            .Include(x => x.FromCurrency)
            .Include(x => x.ToCurrency)
            .AsQueryable();

        if (request.FromCurrencyId.HasValue)
        {
            query = query.Where(x => x.FromCurrencyId == request.FromCurrencyId.Value);
        }

        if (request.ToCurrencyId.HasValue)
        {
            query = query.Where(x => x.ToCurrencyId == request.ToCurrencyId.Value);
        }

        if (request.DateFrom.HasValue)
        {
            query = query.Where(x => x.EffectiveDate >= request.DateFrom.Value);
        }

        if (request.DateTo.HasValue)
        {
            query = query.Where(x => x.EffectiveDate <= request.DateTo.Value);
        }

        return await query
            .OrderByDescending(x => x.EffectiveDate)
            .ThenBy(x => x.FromCurrency.Code)
            .ThenBy(x => x.ToCurrency.Code)
            .Select(x => new ExchangeRateDto
            {
                Id = x.Id,
                FromCurrencyId = x.FromCurrencyId,
                FromCurrencyCode = x.FromCurrency.Code,
                ToCurrencyId = x.ToCurrencyId,
                ToCurrencyCode = x.ToCurrency.Code,
                EffectiveDate = x.EffectiveDate,
                Rate = x.Rate
            })
            .ToListAsync(cancellationToken);
    }
}

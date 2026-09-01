using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.ExchangeRates.Common;

namespace PersonalHub.Application.Features.ExchangeRates.GetExchangeRateById;

public class GetExchangeRateByIdHandler
    : IRequestHandler<GetExchangeRateByIdQuery, ExchangeRateDto?>
{
    private readonly IAppDbContext _context;

    public GetExchangeRateByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ExchangeRateDto?> Handle(
        GetExchangeRateByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.ExchangeRates
            .Include(x => x.FromCurrency)
            .Include(x => x.ToCurrency)
            .Where(x => x.Id == request.Id)
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
            .FirstOrDefaultAsync(cancellationToken);
    }
}

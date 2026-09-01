using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.ExchangeRates.UpdateExchangeRate;

public class UpdateExchangeRateHandler : IRequestHandler<UpdateExchangeRateCommand, Unit>
{
    private readonly IAppDbContext _context;

    public UpdateExchangeRateHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdateExchangeRateCommand request,
        CancellationToken cancellationToken)
    {
        var exchangeRate = await _context.ExchangeRates
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (exchangeRate == null)
        {
            throw new InvalidOperationException($"Exchange rate with ID {request.Id} not found.");
        }

        var duplicate = await _context.ExchangeRates
            .AnyAsync(x =>
                x.Id != request.Id &&
                x.FromCurrencyId == request.FromCurrencyId &&
                x.ToCurrencyId == request.ToCurrencyId &&
                x.EffectiveDate.Date == request.EffectiveDate.Date,
                cancellationToken);

        if (duplicate)
        {
            throw new InvalidOperationException(
                $"An exchange rate already exists for this currency pair on {request.EffectiveDate:yyyy-MM-dd}.");
        }

        exchangeRate.FromCurrencyId = request.FromCurrencyId;
        exchangeRate.ToCurrencyId = request.ToCurrencyId;
        exchangeRate.EffectiveDate = request.EffectiveDate.Date;
        exchangeRate.Rate = request.Rate;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

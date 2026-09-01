using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.ExchangeRates.CreateExchangeRate;

public class CreateExchangeRateHandler : IRequestHandler<CreateExchangeRateCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateExchangeRateHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateExchangeRateCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _context.ExchangeRates
            .AnyAsync(x =>
                x.FromCurrencyId == request.FromCurrencyId &&
                x.ToCurrencyId == request.ToCurrencyId &&
                x.EffectiveDate.Date == request.EffectiveDate.Date,
                cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException(
                $"An exchange rate already exists for this currency pair on {request.EffectiveDate:yyyy-MM-dd}.");
        }

        var exchangeRate = new ExchangeRate
        {
            Id = Guid.NewGuid(),
            FromCurrencyId = request.FromCurrencyId,
            ToCurrencyId = request.ToCurrencyId,
            EffectiveDate = request.EffectiveDate.Date,
            Rate = request.Rate
        };

        _context.ExchangeRates.Add(exchangeRate);
        await _context.SaveChangesAsync(cancellationToken);

        return exchangeRate.Id;
    }
}

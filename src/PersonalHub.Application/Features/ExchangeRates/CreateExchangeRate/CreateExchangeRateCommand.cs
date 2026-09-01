using MediatR;

namespace PersonalHub.Application.Features.ExchangeRates.CreateExchangeRate;

public record CreateExchangeRateCommand(
    Guid FromCurrencyId,
    Guid ToCurrencyId,
    DateTime EffectiveDate,
    decimal Rate
) : IRequest<Guid>;

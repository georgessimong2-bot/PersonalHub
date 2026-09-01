using MediatR;

namespace PersonalHub.Application.Features.ExchangeRates.UpdateExchangeRate;

public record UpdateExchangeRateCommand(
    Guid Id,
    Guid FromCurrencyId,
    Guid ToCurrencyId,
    DateTime EffectiveDate,
    decimal Rate
) : IRequest<Unit>;

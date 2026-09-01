using MediatR;
using PersonalHub.Application.Features.ExchangeRates.Common;

namespace PersonalHub.Application.Features.ExchangeRates.GetExchangeRates;

public record GetExchangeRatesQuery(
    Guid? FromCurrencyId,
    Guid? ToCurrencyId,
    DateTime? DateFrom,
    DateTime? DateTo
) : IRequest<List<ExchangeRateDto>>;

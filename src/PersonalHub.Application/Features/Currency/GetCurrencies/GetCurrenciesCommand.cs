using MediatR;
using PersonalHub.Application.Features.Currency.Common;

namespace PersonalHub.Application.Features.Currency.GetCurrencies;

public record GetCurrenciesCommand
    : IRequest<List<CurrencyDto>>;
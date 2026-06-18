using MediatR;
using PersonalHub.Application.Features.Currency.Common;

namespace PersonalHub.Application.Features.Currency.GetCurrencyById;

public record GetCurrencyByIdCommand(Guid Id)
    : IRequest<CurrencyDto?>;
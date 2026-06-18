using MediatR;

namespace PersonalHub.Application.Features.Currency.DeleteCurrency;

public record DeleteCurrencyCommand(Guid Id)
    : IRequest;
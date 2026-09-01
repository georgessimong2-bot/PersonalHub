using MediatR;

namespace PersonalHub.Application.Features.ExchangeRates.DeleteExchangeRate;

public record DeleteExchangeRateCommand(Guid Id) : IRequest<Unit>;

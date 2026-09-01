using MediatR;
using PersonalHub.Application.Features.ExchangeRates.Common;

namespace PersonalHub.Application.Features.ExchangeRates.GetExchangeRateById;

public record GetExchangeRateByIdQuery(Guid Id) : IRequest<ExchangeRateDto?>;

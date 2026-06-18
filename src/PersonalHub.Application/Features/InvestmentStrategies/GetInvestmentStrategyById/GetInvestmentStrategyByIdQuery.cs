using MediatR;
using PersonalHub.Application.Features.InvestmentStrategies.Common;

namespace PersonalHub.Application.Features.InvestmentStrategies.GetInvestmentStrategyById;

public record GetInvestmentStrategyByIdQuery(Guid Id)
    : IRequest<InvestmentStrategyDto?>;

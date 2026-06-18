using MediatR;
using PersonalHub.Application.Features.InvestmentStrategies.Common;

namespace PersonalHub.Application.Features.InvestmentStrategies.GetInvestmentStrategies;

public record GetInvestmentStrategiesQuery()
    : IRequest<List<InvestmentStrategyDto>>;

using MediatR;

namespace PersonalHub.Application.Features.InvestmentStrategies.DeleteInvestmentStrategy;

public record DeleteInvestmentStrategyCommand(Guid Id)
    : IRequest;

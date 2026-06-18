using MediatR;

namespace PersonalHub.Application.Features.InvestmentStrategies.UpdateInvestmentStrategy;

public record UpdateInvestmentStrategyCommand(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive)
    : IRequest;

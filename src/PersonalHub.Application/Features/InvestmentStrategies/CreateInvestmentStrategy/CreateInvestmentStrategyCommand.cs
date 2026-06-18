using MediatR;

namespace PersonalHub.Application.Features.InvestmentStrategies.CreateInvestmentStrategy;

public class CreateInvestmentStrategyCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}

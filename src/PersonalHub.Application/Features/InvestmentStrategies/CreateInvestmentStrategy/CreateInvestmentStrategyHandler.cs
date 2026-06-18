using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.InvestmentStrategies.CreateInvestmentStrategy;

public class CreateInvestmentStrategyHandler
    : IRequestHandler<CreateInvestmentStrategyCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateInvestmentStrategyHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateInvestmentStrategyCommand request,
        CancellationToken cancellationToken)
    {
        var investmentStrategy = new InvestmentStrategy
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive
        };

        _context.InvestmentStrategies.Add(investmentStrategy);
        await _context.SaveChangesAsync(cancellationToken);

        return investmentStrategy.Id;
    }
}

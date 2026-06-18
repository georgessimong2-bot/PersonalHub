using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.InvestmentStrategies.UpdateInvestmentStrategy;

public class UpdateInvestmentStrategyHandler
    : IRequestHandler<UpdateInvestmentStrategyCommand>
{
    private readonly IAppDbContext _context;

    public UpdateInvestmentStrategyHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateInvestmentStrategyCommand request,
        CancellationToken cancellationToken)
    {
        var investmentStrategy = await _context.InvestmentStrategies
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (investmentStrategy is null)
            throw new Exception("Investment Strategy not found");

        investmentStrategy.Name = request.Name;
        investmentStrategy.Description = request.Description;
        investmentStrategy.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

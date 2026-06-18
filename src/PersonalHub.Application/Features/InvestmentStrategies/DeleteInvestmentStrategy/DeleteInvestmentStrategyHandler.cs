using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.InvestmentStrategies.DeleteInvestmentStrategy;

public class DeleteInvestmentStrategyHandler
    : IRequestHandler<DeleteInvestmentStrategyCommand>
{
    private readonly IAppDbContext _context;

    public DeleteInvestmentStrategyHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteInvestmentStrategyCommand request,
        CancellationToken cancellationToken)
    {
        var investmentStrategy = await _context.InvestmentStrategies
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (investmentStrategy is null)
            throw new Exception("Investment Strategy not found");

        _context.InvestmentStrategies.Remove(investmentStrategy);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

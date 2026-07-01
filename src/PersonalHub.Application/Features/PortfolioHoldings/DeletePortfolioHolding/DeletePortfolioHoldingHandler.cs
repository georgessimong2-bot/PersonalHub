using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.PortfolioHoldings.DeletePortfolioHolding;

public class DeletePortfolioHoldingHandler
    : IRequestHandler<DeletePortfolioHoldingCommand>
{
    private readonly IAppDbContext _context;

    public DeletePortfolioHoldingHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeletePortfolioHoldingCommand request,
        CancellationToken cancellationToken)
    {
        var holding = await _context.PortfolioHoldings
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new Exception("Portfolio holding not found");

        _context.PortfolioHoldings.Remove(holding);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

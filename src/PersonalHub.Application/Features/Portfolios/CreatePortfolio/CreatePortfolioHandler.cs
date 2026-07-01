using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.Portfolios.CreatePortfolio;

public class CreatePortfolioHandler
    : IRequestHandler<CreatePortfolioCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreatePortfolioHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreatePortfolioCommand request,
        CancellationToken cancellationToken)
    {
        var portfolio = new Portfolio
        {
            Id = Guid.NewGuid(),
            ShareClassId = request.ShareClassId,
            ValuationDate = request.ValuationDate,
            IsActive = request.IsActive
        };

        _context.Portfolios.Add(portfolio);

        await _context.SaveChangesAsync(cancellationToken);

        return portfolio.Id;
    }
}

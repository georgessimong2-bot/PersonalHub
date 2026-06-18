using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.InvestmentStrategies.Common;

namespace PersonalHub.Application.Features.InvestmentStrategies.GetInvestmentStrategies;

public class GetInvestmentStrategiesHandler
    : IRequestHandler<GetInvestmentStrategiesQuery, List<InvestmentStrategyDto>>
{
    private readonly IAppDbContext _context;

    public GetInvestmentStrategiesHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<InvestmentStrategyDto>> Handle(
        GetInvestmentStrategiesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.InvestmentStrategies
            .Select(x => new InvestmentStrategyDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}

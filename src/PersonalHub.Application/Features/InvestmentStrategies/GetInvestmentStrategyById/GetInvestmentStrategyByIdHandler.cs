using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.InvestmentStrategies.Common;

namespace PersonalHub.Application.Features.InvestmentStrategies.GetInvestmentStrategyById;

public class GetInvestmentStrategyByIdHandler
    : IRequestHandler<GetInvestmentStrategyByIdQuery, InvestmentStrategyDto?>
{
    private readonly IAppDbContext _context;

    public GetInvestmentStrategyByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<InvestmentStrategyDto?> Handle(
        GetInvestmentStrategyByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.InvestmentStrategies
            .Where(x => x.Id == request.Id)
            .Select(x => new InvestmentStrategyDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}

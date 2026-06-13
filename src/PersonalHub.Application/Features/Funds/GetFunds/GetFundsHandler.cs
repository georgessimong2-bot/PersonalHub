using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Funds.Common;

namespace PersonalHub.Application.Features.Funds.GetFunds;

public class GetFundsHandler
    : IRequestHandler<GetFundsCommand, List<FundDto>>
{
    private readonly IAppDbContext _context;

    public GetFundsHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FundDto>> Handle(
        GetFundsCommand request,
        CancellationToken cancellationToken)
    {
        return await _context.Funds
            .Include(x => x.FundType)
            .Select(x => new FundDto
            {
                Id = x.Id,
                Name = x.Name,
                FundTypeId = x.FundTypeId,
                FundTypeName = x.FundType.Name
            })
            .ToListAsync(cancellationToken);
    }
}
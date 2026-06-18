using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.SubFunds.Common;

namespace PersonalHub.Application.Features.SubFunds.GetSubFunds;

public class GetSubFundsHandler
    : IRequestHandler<GetSubFundsQuery, List<SubFundDto>>
{
    private readonly IAppDbContext _context;

    public GetSubFundsHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SubFundDto>> Handle(
        GetSubFundsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.SubFunds
            .Select(x => new SubFundDto
            {
                Id = x.Id,
                FundId = x.FundId,
                BenchmarkId = x.BenchmarkId,
                AssetClassId = x.AssetClassId,
                SfdrClassificationId = x.SfdrClassificationId,
                Name = x.Name,
                InternalCode = x.InternalCode,
                InvestmentObjective = x.InvestmentObjective,
                InvestmentPolicy = x.InvestmentPolicy,
                GeographicFocus = x.GeographicFocus,
                SectorFocus = x.SectorFocus,
                RiskProfile = x.RiskProfile,
                Description = x.Description
            })
            .ToListAsync(cancellationToken);
    }
}

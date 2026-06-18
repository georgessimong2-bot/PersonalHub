using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.SubFunds.Common;

namespace PersonalHub.Application.Features.SubFunds.GetSubFundById;

public class GetSubFundByIdHandler
    : IRequestHandler<GetSubFundByIdQuery, SubFundDto?>
{
    private readonly IAppDbContext _context;

    public GetSubFundByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<SubFundDto?> Handle(
        GetSubFundByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.SubFunds
            .Where(x => x.Id == request.Id)
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
            .FirstOrDefaultAsync(cancellationToken);
    }
}

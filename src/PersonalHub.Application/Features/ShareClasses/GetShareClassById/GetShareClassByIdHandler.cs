using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.ShareClasses.Common;

namespace PersonalHub.Application.Features.ShareClasses.GetShareClassById;

public class GetShareClassByIdHandler
    : IRequestHandler<GetShareClassByIdQuery, ShareClassDto?>
{
    private readonly IAppDbContext _context;

    public GetShareClassByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ShareClassDto?> Handle(
        GetShareClassByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.ShareClasses
            .Where(x => x.Id == request.Id)
            .Select(x => new ShareClassDto
            {
                Id = x.Id,
                SubFundId = x.SubFundId,
                CurrencyId = x.CurrencyId,
                Name = x.Name,
                ISIN = x.ISIN,
                IsHedged = x.IsHedged,
                IsDistribution = x.IsDistribution,
                IsInstitutional = x.IsInstitutional,
                ManagementFee = x.ManagementFee,
                PerformanceFee = x.PerformanceFee,
                MinimumInvestment = x.MinimumInvestment,
                LaunchDate = x.LaunchDate,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}

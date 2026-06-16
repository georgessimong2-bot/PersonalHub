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
                LegalName = x.LegalName,
                FundCode = x.FundCode,

                DomicileCountry = x.DomicileCountry,
                BaseCurrency = x.BaseCurrency,

                LaunchDate = x.LaunchDate,
                IsActive = x.IsActive,

                Description = x.Description,

                FundTypeId = x.FundTypeId,
                FundTypeName = x.FundType.Name,

                SubFundCount = x.SubFunds.Count
            })
            .ToListAsync(cancellationToken);
    }
}
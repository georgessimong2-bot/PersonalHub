using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Funds.Common;

namespace PersonalHub.Application.Features.Funds.GetFundById;

public class GetFundByIdHandler
    : IRequestHandler<GetFundByIdCommand, FundDto?>
{
    private readonly IAppDbContext _context;

    public GetFundByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<FundDto?> Handle(
        GetFundByIdCommand request,
        CancellationToken cancellationToken)
    {
        return await _context.Funds
    .Include(x => x.FundType)
    .Include(x => x.SubFunds)
    .Where(x => x.Id == request.Id)
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
    .FirstOrDefaultAsync(cancellationToken);
    }
}
using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.SubFunds.CreateSubFund;

public class CreateSubFundHandler
    : IRequestHandler<CreateSubFundCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateSubFundHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateSubFundCommand request,
        CancellationToken cancellationToken)
    {
        var subFund = new SubFund
        {
            Id = Guid.NewGuid(),
            FundId = request.FundId,
            BenchmarkId = request.BenchmarkId,
            AssetClassId = request.AssetClassId,
            SfdrClassificationId = request.SfdrClassificationId,
            Name = request.Name,
            InternalCode = request.InternalCode,
            InvestmentObjective = request.InvestmentObjective,
            InvestmentPolicy = request.InvestmentPolicy,
            GeographicFocus = request.GeographicFocus,
            SectorFocus = request.SectorFocus,
            RiskProfile = request.RiskProfile,
            Description = request.Description
        };

        _context.SubFunds.Add(subFund);
        await _context.SaveChangesAsync(cancellationToken);

        return subFund.Id;
    }
}

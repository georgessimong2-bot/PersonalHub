using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.SubFunds.UpdateSubFund;

public class UpdateSubFundHandler
    : IRequestHandler<UpdateSubFundCommand>
{
    private readonly IAppDbContext _context;

    public UpdateSubFundHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateSubFundCommand request,
        CancellationToken cancellationToken)
    {
        var subFund = await _context.SubFunds
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (subFund is null)
            throw new Exception("Sub Fund not found");

        subFund.Name = request.Name;
        subFund.InternalCode = request.InternalCode;
        subFund.InvestmentObjective = request.InvestmentObjective;
        subFund.InvestmentPolicy = request.InvestmentPolicy;
        subFund.GeographicFocus = request.GeographicFocus;
        subFund.SectorFocus = request.SectorFocus;
        subFund.RiskProfile = request.RiskProfile;
        subFund.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

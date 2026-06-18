using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.ShareClasses.UpdateShareClass;

public class UpdateShareClassHandler
    : IRequestHandler<UpdateShareClassCommand>
{
    private readonly IAppDbContext _context;

    public UpdateShareClassHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateShareClassCommand request,
        CancellationToken cancellationToken)
    {
        var shareClass = await _context.ShareClasses
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (shareClass is null)
            throw new Exception("Share Class not found");

        shareClass.Name = request.Name;
        shareClass.ISIN = request.ISIN;
        shareClass.IsHedged = request.IsHedged;
        shareClass.IsDistribution = request.IsDistribution;
        shareClass.IsInstitutional = request.IsInstitutional;
        shareClass.ManagementFee = request.ManagementFee;
        shareClass.PerformanceFee = request.PerformanceFee;
        shareClass.MinimumInvestment = request.MinimumInvestment;
        shareClass.LaunchDate = request.LaunchDate;
        shareClass.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.ShareClasses.CreateShareClass;

public class CreateShareClassHandler
    : IRequestHandler<CreateShareClassCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateShareClassHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateShareClassCommand request,
        CancellationToken cancellationToken)
    {
        var shareClass = new ShareClass
        {
            Id = Guid.NewGuid(),
            SubFundId = request.SubFundId,
            CurrencyId = request.CurrencyId,
            Name = request.Name,
            ISIN = request.ISIN,
            IsHedged = request.IsHedged,
            IsDistribution = request.IsDistribution,
            IsInstitutional = request.IsInstitutional,
            ManagementFee = request.ManagementFee,
            PerformanceFee = request.PerformanceFee,
            MinimumInvestment = request.MinimumInvestment,
            LaunchDate = request.LaunchDate,
            IsActive = request.IsActive
        };

        _context.ShareClasses.Add(shareClass);
        await _context.SaveChangesAsync(cancellationToken);

        return shareClass.Id;
    }
}

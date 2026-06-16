using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Funds.UpdateFund;

public class UpdateFundHandler
    : IRequestHandler<UpdateFundCommand>
{
    private readonly IAppDbContext _context;

    public UpdateFundHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateFundCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Funds
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new Exception("Fund not found");

        entity.Name = request.Name;
        entity.LegalName = request.LegalName;
        entity.FundCode = request.FundCode;

        entity.DomicileCountry = request.DomicileCountry;
        entity.BaseCurrency = request.BaseCurrency;

        entity.LaunchDate = request.LaunchDate;
        entity.IsActive = request.IsActive;

        entity.Description = request.Description;

        entity.FundTypeId = request.FundTypeId;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
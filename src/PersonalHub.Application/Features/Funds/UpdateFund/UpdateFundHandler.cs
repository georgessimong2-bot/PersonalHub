using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Exceptions;
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

        if (request.FundTypeId == Guid.Empty)
            throw new BusinessException("Fund type is required.");

        var fundTypeExists = await _context.FundTypes
            .AnyAsync(x => x.Id == request.FundTypeId, cancellationToken);

        if (!fundTypeExists)
            throw new BusinessException("Selected fund type does not exist.");

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

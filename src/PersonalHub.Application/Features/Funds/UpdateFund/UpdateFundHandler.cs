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
        entity.FundTypeId = request.FundTypeId;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
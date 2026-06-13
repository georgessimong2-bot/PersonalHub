using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.FundTypes.UpdateFundType;

public class UpdateFundTypeHandler
    : IRequestHandler<UpdateFundTypeCommand>
{
    private readonly IAppDbContext _context;

    public UpdateFundTypeHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateFundTypeCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.FundTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new Exception("FundType not found");

        entity.Name = request.Name;
        entity.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
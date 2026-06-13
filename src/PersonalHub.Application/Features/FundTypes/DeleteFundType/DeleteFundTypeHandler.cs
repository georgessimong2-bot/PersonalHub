using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.FundTypes.DeleteFundType;

public class DeleteFundTypeHandler
    : IRequestHandler<DeleteFundTypeCommand>
{
    private readonly IAppDbContext _context;

    public DeleteFundTypeHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteFundTypeCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.FundTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new Exception("FundType not found");

        _context.FundTypes.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
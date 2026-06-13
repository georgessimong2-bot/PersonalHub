using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Funds.DeleteFund;

public class DeleteFundHandler
    : IRequestHandler<DeleteFundCommand>
{
    private readonly IAppDbContext _context;

    public DeleteFundHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteFundCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Funds
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new Exception("Fund not found");

        _context.Funds.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
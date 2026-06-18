using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.SubFunds.DeleteSubFund;

public class DeleteSubFundHandler
    : IRequestHandler<DeleteSubFundCommand>
{
    private readonly IAppDbContext _context;

    public DeleteSubFundHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteSubFundCommand request,
        CancellationToken cancellationToken)
    {
        var subFund = await _context.SubFunds
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (subFund is null)
            throw new Exception("Sub Fund not found");

        _context.SubFunds.Remove(subFund);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

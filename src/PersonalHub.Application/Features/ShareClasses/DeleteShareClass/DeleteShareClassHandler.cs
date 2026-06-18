using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.ShareClasses.DeleteShareClass;

public class DeleteShareClassHandler
    : IRequestHandler<DeleteShareClassCommand>
{
    private readonly IAppDbContext _context;

    public DeleteShareClassHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteShareClassCommand request,
        CancellationToken cancellationToken)
    {
        var shareClass = await _context.ShareClasses
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (shareClass is null)
            throw new Exception("Share Class not found");

        _context.ShareClasses.Remove(shareClass);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

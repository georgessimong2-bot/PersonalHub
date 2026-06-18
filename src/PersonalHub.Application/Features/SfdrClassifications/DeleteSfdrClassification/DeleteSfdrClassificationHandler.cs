using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.SfdrClassifications.DeleteSfdrClassification;

public class DeleteSfdrClassificationHandler
    : IRequestHandler<DeleteSfdrClassificationCommand>
{
    private readonly IAppDbContext _context;

    public DeleteSfdrClassificationHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteSfdrClassificationCommand request,
        CancellationToken cancellationToken)
    {
        var sfdrClassification = await _context.SfdrClassifications
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (sfdrClassification is null)
            throw new Exception("SFDR Classification not found");

        _context.SfdrClassifications.Remove(sfdrClassification);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

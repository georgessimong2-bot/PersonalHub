using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.SfdrClassifications.UpdateSfdrClassification;

public class UpdateSfdrClassificationHandler
    : IRequestHandler<UpdateSfdrClassificationCommand>
{
    private readonly IAppDbContext _context;

    public UpdateSfdrClassificationHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateSfdrClassificationCommand request,
        CancellationToken cancellationToken)
    {
        var sfdrClassification = await _context.SfdrClassifications
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (sfdrClassification is null)
            throw new Exception("SFDR Classification not found");

        sfdrClassification.Name = request.Name;
        sfdrClassification.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

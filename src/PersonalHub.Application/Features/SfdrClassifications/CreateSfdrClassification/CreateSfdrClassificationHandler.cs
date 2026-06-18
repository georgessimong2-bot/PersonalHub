using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.SfdrClassifications.CreateSfdrClassification;

public class CreateSfdrClassificationHandler
    : IRequestHandler<CreateSfdrClassificationCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateSfdrClassificationHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateSfdrClassificationCommand request,
        CancellationToken cancellationToken)
    {
        var sfdrClassification = new SfdrClassification
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        _context.SfdrClassifications.Add(sfdrClassification);
        await _context.SaveChangesAsync(cancellationToken);

        return sfdrClassification.Id;
    }
}

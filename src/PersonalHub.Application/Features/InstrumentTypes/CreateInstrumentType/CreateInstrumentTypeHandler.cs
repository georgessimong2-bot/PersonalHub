using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.InstrumentTypes.CreateInstrumentType;

public class CreateInstrumentTypeHandler
    : IRequestHandler<CreateInstrumentTypeCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateInstrumentTypeHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateInstrumentTypeCommand request,
        CancellationToken cancellationToken)
    {
        var instrumentType = new InstrumentType
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive
        };

        _context.InstrumentTypes.Add(instrumentType);
        await _context.SaveChangesAsync(cancellationToken);

        return instrumentType.Id;
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.InstrumentTypes.UpdateInstrumentType;

public class UpdateInstrumentTypeHandler
    : IRequestHandler<UpdateInstrumentTypeCommand>
{
    private readonly IAppDbContext _context;

    public UpdateInstrumentTypeHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateInstrumentTypeCommand request,
        CancellationToken cancellationToken)
    {
        var instrumentType = await _context.InstrumentTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (instrumentType is null)
            throw new Exception("Instrument Type not found");

        instrumentType.Name = request.Name;
        instrumentType.Description = request.Description;
        instrumentType.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

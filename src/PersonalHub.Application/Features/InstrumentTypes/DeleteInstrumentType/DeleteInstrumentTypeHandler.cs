using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.InstrumentTypes.DeleteInstrumentType;

public class DeleteInstrumentTypeHandler
    : IRequestHandler<DeleteInstrumentTypeCommand>
{
    private readonly IAppDbContext _context;

    public DeleteInstrumentTypeHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteInstrumentTypeCommand request,
        CancellationToken cancellationToken)
    {
        var instrumentType = await _context.InstrumentTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (instrumentType is null)
            throw new Exception("Instrument Type not found");

        _context.InstrumentTypes.Remove(instrumentType);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

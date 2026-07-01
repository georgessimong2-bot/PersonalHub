using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Instruments.DeleteInstrument;

public class DeleteInstrumentHandler
    : IRequestHandler<DeleteInstrumentCommand>
{
    private readonly IAppDbContext _context;

    public DeleteInstrumentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteInstrumentCommand request,
        CancellationToken cancellationToken)
    {
        var instrument = await _context.Instruments
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (instrument is null)
            throw new Exception("Instrument not found");

        _context.Instruments.Remove(instrument);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.InstrumentPrices.DeleteInstrumentPrice;

public class DeleteInstrumentPriceHandler
    : IRequestHandler<DeleteInstrumentPriceCommand>
{
    private readonly IAppDbContext _context;

    public DeleteInstrumentPriceHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteInstrumentPriceCommand request,
        CancellationToken cancellationToken)
    {
        var instrumentPrice = await _context.InstrumentPrices
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new Exception("Instrument price not found");

        _context.InstrumentPrices.Remove(instrumentPrice);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

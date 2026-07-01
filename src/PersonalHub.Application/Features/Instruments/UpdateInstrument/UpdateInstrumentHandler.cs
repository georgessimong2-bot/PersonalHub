using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Instruments.UpdateInstrument;

public class UpdateInstrumentHandler
    : IRequestHandler<UpdateInstrumentCommand>
{
    private readonly IAppDbContext _context;

    public UpdateInstrumentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateInstrumentCommand request,
        CancellationToken cancellationToken)
    {
        var instrument = await _context.Instruments
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (instrument is null)
            throw new Exception("Instrument not found");

        instrument.InstrumentTypeId = request.InstrumentTypeId;
        instrument.CurrencyId = request.CurrencyId;
        instrument.Name = request.Name;
        instrument.ISIN = request.ISIN;
        instrument.Ticker = request.Ticker;
        instrument.Issuer = request.Issuer;
        instrument.CountryOfRisk = request.CountryOfRisk;
        instrument.Sector = request.Sector;
        instrument.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

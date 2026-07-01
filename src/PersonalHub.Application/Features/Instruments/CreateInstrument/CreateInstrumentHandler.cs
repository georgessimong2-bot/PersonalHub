using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.Instruments.CreateInstrument;

public class CreateInstrumentHandler
    : IRequestHandler<CreateInstrumentCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateInstrumentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateInstrumentCommand request,
        CancellationToken cancellationToken)
    {
        var instrument = new Instrument
        {
            Id = Guid.NewGuid(),
            InstrumentTypeId = request.InstrumentTypeId,
            CurrencyId = request.CurrencyId,
            Name = request.Name,
            ISIN = request.ISIN,
            Ticker = request.Ticker,
            Issuer = request.Issuer,
            CountryOfRisk = request.CountryOfRisk,
            Sector = request.Sector,
            IsActive = request.IsActive
        };

        _context.Instruments.Add(instrument);
        await _context.SaveChangesAsync(cancellationToken);

        return instrument.Id;
    }
}

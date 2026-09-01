using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Instruments.Common;

namespace PersonalHub.Application.Features.Instruments.GetInstruments;

public class GetInstrumentsHandler
    : IRequestHandler<GetInstrumentsQuery, List<InstrumentDto>>
{
    private readonly IAppDbContext _context;

    public GetInstrumentsHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<InstrumentDto>> Handle(
        GetInstrumentsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Instruments
            .Select(x => new InstrumentDto
            {
                Id = x.Id,
                InstrumentTypeId = x.InstrumentTypeId,
                CurrencyId = x.CurrencyId,
                CurrencyCode = x.Currency.Code,
                Name = x.Name,
                ISIN = x.ISIN,
                Ticker = x.Ticker,
                Issuer = x.Issuer,
                CountryOfRisk = x.CountryOfRisk,
                Sector = x.Sector,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}

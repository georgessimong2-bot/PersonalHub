using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Instruments.Common;

namespace PersonalHub.Application.Features.Instruments.GetInstrumentById;

public class GetInstrumentByIdHandler
    : IRequestHandler<GetInstrumentByIdQuery, InstrumentDto?>
{
    private readonly IAppDbContext _context;

    public GetInstrumentByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<InstrumentDto?> Handle(
        GetInstrumentByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Instruments
            .Where(x => x.Id == request.Id)
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
            .FirstOrDefaultAsync(cancellationToken);
    }
}

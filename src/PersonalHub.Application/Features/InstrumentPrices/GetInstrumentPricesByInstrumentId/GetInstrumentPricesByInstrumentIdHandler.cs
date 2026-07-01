using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.InstrumentPrices.Common;

namespace PersonalHub.Application.Features.InstrumentPrices.GetInstrumentPricesByInstrumentId;

public class GetInstrumentPricesByInstrumentIdHandler
    : IRequestHandler<GetInstrumentPricesByInstrumentIdQuery, List<InstrumentPriceDto>>
{
    private readonly IAppDbContext _context;

    public GetInstrumentPricesByInstrumentIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<InstrumentPriceDto>> Handle(
        GetInstrumentPricesByInstrumentIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.InstrumentPrices
            .Where(x => x.InstrumentId == request.InstrumentId)
            .OrderByDescending(x => x.PriceDate)
            .Select(x => new InstrumentPriceDto
            {
                Id = x.Id,
                InstrumentId = x.InstrumentId,
                Price = x.Price,
                PriceDate = x.PriceDate
            })
            .ToListAsync(cancellationToken);
    }
}

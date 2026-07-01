using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.InstrumentPrices.CreateInstrumentPrice;

public class CreateInstrumentPriceHandler
    : IRequestHandler<CreateInstrumentPriceCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateInstrumentPriceHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateInstrumentPriceCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await _context.InstrumentPrices
            .FirstOrDefaultAsync(
                x => x.InstrumentId == request.InstrumentId && x.PriceDate == request.PriceDate,
                cancellationToken);

        if (existing is not null)
        {
            existing.Price = request.Price;
            await _context.SaveChangesAsync(cancellationToken);
            return existing.Id;
        }

        var instrumentPrice = new InstrumentPrice
        {
            Id = Guid.NewGuid(),
            InstrumentId = request.InstrumentId,
            Price = request.Price,
            PriceDate = request.PriceDate
        };

        _context.InstrumentPrices.Add(instrumentPrice);

        await _context.SaveChangesAsync(cancellationToken);

        return instrumentPrice.Id;
    }
}

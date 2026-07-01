using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Exceptions;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.InstrumentPrices.UpdateInstrumentPrice;

public class UpdateInstrumentPriceHandler
    : IRequestHandler<UpdateInstrumentPriceCommand>
{
    private readonly IAppDbContext _context;

    public UpdateInstrumentPriceHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateInstrumentPriceCommand request,
        CancellationToken cancellationToken)
    {
        var instrumentPrice = await _context.InstrumentPrices
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new BusinessException("Instrument price not found");

        var conflictingPrice = await _context.InstrumentPrices
            .AnyAsync(
                x =>
                    x.Id != request.Id &&
                    x.InstrumentId == instrumentPrice.InstrumentId &&
                    x.PriceDate == request.PriceDate,
                cancellationToken);

        if (conflictingPrice)
            throw new BusinessException("A price already exists for this instrument on the selected date");

        instrumentPrice.Price = request.Price;
        instrumentPrice.PriceDate = request.PriceDate;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

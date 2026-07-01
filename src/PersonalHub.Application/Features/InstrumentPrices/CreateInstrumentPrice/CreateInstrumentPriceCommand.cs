using MediatR;

namespace PersonalHub.Application.Features.InstrumentPrices.CreateInstrumentPrice;

public class CreateInstrumentPriceCommand : IRequest<Guid>
{
    public Guid InstrumentId { get; set; }

    public decimal Price { get; set; }

    public DateTime PriceDate { get; set; }
}

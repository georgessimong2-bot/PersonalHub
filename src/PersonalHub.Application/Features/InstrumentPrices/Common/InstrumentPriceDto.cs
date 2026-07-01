namespace PersonalHub.Application.Features.InstrumentPrices.Common;

public class InstrumentPriceDto
{
    public Guid Id { get; set; }

    public Guid InstrumentId { get; set; }

    public decimal Price { get; set; }

    public DateTime PriceDate { get; set; }
}

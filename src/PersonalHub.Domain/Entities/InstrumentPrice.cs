namespace PersonalHub.Domain.Entities;

public class InstrumentPrice : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public Guid InstrumentId { get; set; }

    public decimal Price { get; set; }

    public DateTime PriceDate { get; set; }

    public Instrument Instrument { get; set; } = null!;
}

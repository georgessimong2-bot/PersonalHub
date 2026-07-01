namespace PersonalHub.Domain.Entities;

public class PortfolioHolding : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public Guid PortfolioId { get; set; }

    public Guid InstrumentId { get; set; }

    public decimal Quantity { get; set; }

    public decimal? AverageCost { get; set; }

    public decimal? MarketValue { get; set; }

    public Portfolio Portfolio { get; set; } = null!;

    public Instrument Instrument { get; set; } = null!;
}

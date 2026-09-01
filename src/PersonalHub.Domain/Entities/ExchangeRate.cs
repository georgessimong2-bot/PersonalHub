namespace PersonalHub.Domain.Entities;

public class ExchangeRate : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public Guid FromCurrencyId { get; set; }

    public Guid ToCurrencyId { get; set; }

    public DateTime EffectiveDate { get; set; }

    public decimal Rate { get; set; }

    public Currency FromCurrency { get; set; } = null!;

    public Currency ToCurrency { get; set; } = null!;
}

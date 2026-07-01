namespace PersonalHub.Domain.Entities;

public class Instrument : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public Guid InstrumentTypeId { get; set; }

    public Guid CurrencyId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ISIN { get; set; } = string.Empty;

    public string? Ticker { get; set; }

    public string? Issuer { get; set; }

    public string? CountryOfRisk { get; set; }

    public string? Sector { get; set; }

    public bool IsActive { get; set; } = true;

    public InstrumentType InstrumentType { get; set; } = null!;

    public Currency Currency { get; set; } = null!;
}

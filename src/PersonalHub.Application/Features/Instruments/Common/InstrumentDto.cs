namespace PersonalHub.Application.Features.Instruments.Common;

public class InstrumentDto
{
    public Guid Id { get; set; }

    public Guid InstrumentTypeId { get; set; }

    public Guid CurrencyId { get; set; }

    public string CurrencyCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string ISIN { get; set; } = string.Empty;

    public string? Ticker { get; set; }

    public string? Issuer { get; set; }

    public string? CountryOfRisk { get; set; }

    public string? Sector { get; set; }

    public bool IsActive { get; set; }
}

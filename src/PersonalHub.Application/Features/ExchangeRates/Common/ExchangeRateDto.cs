namespace PersonalHub.Application.Features.ExchangeRates.Common;

public class ExchangeRateDto
{
    public Guid Id { get; set; }

    public Guid FromCurrencyId { get; set; }

    public string FromCurrencyCode { get; set; } = string.Empty;

    public Guid ToCurrencyId { get; set; }

    public string ToCurrencyCode { get; set; } = string.Empty;

    public DateTime EffectiveDate { get; set; }

    public decimal Rate { get; set; }
}

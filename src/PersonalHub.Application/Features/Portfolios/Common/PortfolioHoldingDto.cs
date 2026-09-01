namespace PersonalHub.Application.Features.Portfolios.Common;

public class PortfolioHoldingDto
{
    public Guid Id { get; set; }

    public Guid PortfolioId { get; set; }

    public Guid InstrumentId { get; set; }

    public string InstrumentName { get; set; } = string.Empty;

    public string InstrumentISIN { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public decimal? AverageCost { get; set; }

    public decimal? MarketValue { get; set; }

    public string? InstrumentCurrencyCode { get; set; }

    public decimal? MarketValueInSubFundCurrency { get; set; }

    public bool HasExchangeRate { get; set; }
}

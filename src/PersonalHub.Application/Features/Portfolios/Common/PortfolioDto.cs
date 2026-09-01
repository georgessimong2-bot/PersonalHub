namespace PersonalHub.Application.Features.Portfolios.Common;

public class PortfolioDto
{
    public Guid Id { get; set; }

    public Guid ShareClassId { get; set; }

    public string ShareClassName { get; set; } = string.Empty;

    public string SubFundName { get; set; } = string.Empty;

    public string FundName { get; set; } = string.Empty;

    public DateTime ValuationDate { get; set; }

    public bool IsActive { get; set; }

    public int HoldingsCount { get; set; }

    public decimal? TotalMarketValue { get; set; }

    public string? SubFundCurrencyCode { get; set; }

    public decimal? TotalMarketValueInSubFundCurrency { get; set; }

    public List<PortfolioHoldingDto> Holdings { get; set; } = [];
}

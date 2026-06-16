using PersonalHub.Domain.Entities;

public class ShareClass : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public Guid SubFundId { get; set; }

    public SubFund SubFund { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public string ISIN { get; set; } = string.Empty;

    public string Currency { get; set; } = "EUR";

    public bool Hedged { get; set; }

    public bool Distributing { get; set; }

    public string InvestorType { get; set; } = string.Empty;

    public decimal? ManagementFee { get; set; }

    public decimal? MinimumInvestment { get; set; }

    public bool IsActive { get; set; } = true;
}
using PersonalHub.Domain.Entities;

public class ShareClass : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public Guid SubFundId { get; set; }

    public Guid CurrencyId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ISIN { get; set; } = string.Empty;

    public bool IsHedged { get; set; }

    public bool IsDistribution { get; set; }

    public bool IsInstitutional { get; set; }

    public decimal? ManagementFee { get; set; }

    public decimal? PerformanceFee { get; set; }

    public decimal? MinimumInvestment { get; set; }

    public DateTime? LaunchDate { get; set; }

    public bool IsActive { get; set; } = true;

    public SubFund SubFund { get; set; } = null!;

    public Currency Currency { get; set; } = null!;

    public ICollection<Portfolio> Portfolios { get; set; } = [];
}
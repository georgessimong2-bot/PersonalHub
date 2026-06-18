namespace PersonalHub.Application.Features.SubFunds.Common;

public class SubFundDto
{
    public Guid Id { get; set; }

    public Guid FundId { get; set; }

    public Guid? BenchmarkId { get; set; }

    public Guid? InvestmentStrategyId { get; set; }

    public Guid? SfdrClassificationId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? InternalCode { get; set; }

    public string? InvestmentObjective { get; set; }

    public string? InvestmentPolicy { get; set; }

    public string? GeographicFocus { get; set; }

    public string? SectorFocus { get; set; }

    public string? RiskProfile { get; set; }

    public string? Description { get; set; }
}

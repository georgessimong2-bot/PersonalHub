using PersonalHub.Domain.Entities;

public class SubFund : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public Guid FundId { get; set; }

    public Guid? BenchmarkId { get; set; }

    public Guid? AssetClassId { get; set; }

    public Guid? SfdrClassificationId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? InternalCode { get; set; }

    public string? InvestmentObjective { get; set; }

    public string? InvestmentPolicy { get; set; }

    public string? GeographicFocus { get; set; }

    public string? SectorFocus { get; set; }

    public string? RiskProfile { get; set; }

    public string? Description { get; set; }

    public DateTime? LaunchDate { get; set; }

    public DateTime? OnboardingDate { get; set; }

    public bool IsActive { get; set; } = true;

    public Fund Fund { get; set; } = null!;

    public Benchmark? Benchmark { get; set; }

    public AssetClass? AssetClass { get; set; }

    public SfdrClassification? SfdrClassification { get; set; }

    public ICollection<ShareClass> ShareClasses { get; set; } = [];
}
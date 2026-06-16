using PersonalHub.Domain.Entities;

public class SubFund : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public Guid FundId { get; set; }

    public Fund Fund { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public string? InvestmentObjective { get; set; }

    public string? Benchmark { get; set; }

    public string Currency { get; set; } = "EUR";

    public DateTime? LaunchDate { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<ShareClass> ShareClasses { get; set; }
        = new List<ShareClass>();
}
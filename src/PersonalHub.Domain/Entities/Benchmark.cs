namespace PersonalHub.Domain.Entities;

public class Benchmark : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? BloombergTicker { get; set; }

    public string? ReutersCode { get; set; }

    public string? Provider { get; set; }

    public Guid? CurrencyId { get; set; }

    public Currency? Currency { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<SubFund> SubFunds { get; set; } = [];
}
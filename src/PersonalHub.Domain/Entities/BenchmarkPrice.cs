namespace PersonalHub.Domain.Entities;

public class BenchmarkPrice : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public Guid BenchmarkId { get; set; }

    public decimal Price { get; set; }

    public DateTime PriceDate { get; set; }

    public Benchmark Benchmark { get; set; } = null!;
}

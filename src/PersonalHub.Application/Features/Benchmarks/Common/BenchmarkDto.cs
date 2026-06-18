namespace PersonalHub.Application.Features.Benchmarks.Common;

public class BenchmarkDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? BloombergTicker { get; set; }

    public string? ReutersCode { get; set; }

    public string? Provider { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}

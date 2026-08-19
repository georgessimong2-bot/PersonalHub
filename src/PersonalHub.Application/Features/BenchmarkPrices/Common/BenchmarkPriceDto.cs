namespace PersonalHub.Application.Features.BenchmarkPrices.Common;

public class BenchmarkPriceDto
{
    public Guid Id { get; set; }

    public Guid BenchmarkId { get; set; }

    public decimal Price { get; set; }

    public DateTime PriceDate { get; set; }
}

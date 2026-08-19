using MediatR;

namespace PersonalHub.Application.Features.BenchmarkPrices.CreateBenchmarkPrice;

public class CreateBenchmarkPriceCommand : IRequest<Guid>
{
    public Guid BenchmarkId { get; set; }

    public decimal Price { get; set; }

    public DateTime PriceDate { get; set; }
}

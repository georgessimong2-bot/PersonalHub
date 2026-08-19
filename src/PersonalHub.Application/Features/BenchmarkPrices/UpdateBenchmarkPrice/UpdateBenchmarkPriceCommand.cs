using MediatR;

namespace PersonalHub.Application.Features.BenchmarkPrices.UpdateBenchmarkPrice;

public record UpdateBenchmarkPriceCommand(
    Guid Id,
    decimal Price,
    DateTime PriceDate)
    : IRequest;

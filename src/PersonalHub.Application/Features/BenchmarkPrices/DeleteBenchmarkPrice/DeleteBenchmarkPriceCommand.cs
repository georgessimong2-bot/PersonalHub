using MediatR;

namespace PersonalHub.Application.Features.BenchmarkPrices.DeleteBenchmarkPrice;

public record DeleteBenchmarkPriceCommand(Guid Id) : IRequest;

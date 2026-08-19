using MediatR;
using PersonalHub.Application.Features.BenchmarkPrices.Common;

namespace PersonalHub.Application.Features.BenchmarkPrices.GetBenchmarkPricesByBenchmarkId;

public record GetBenchmarkPricesByBenchmarkIdQuery(Guid BenchmarkId)
    : IRequest<List<BenchmarkPriceDto>>;

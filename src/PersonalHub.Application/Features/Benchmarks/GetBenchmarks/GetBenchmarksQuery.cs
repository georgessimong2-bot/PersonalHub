using MediatR;
using PersonalHub.Application.Features.Benchmarks.Common;

namespace PersonalHub.Application.Features.Benchmarks.GetBenchmarks;

public record GetBenchmarksQuery()
    : IRequest<List<BenchmarkDto>>;

using MediatR;
using PersonalHub.Application.Features.Benchmarks.Common;

namespace PersonalHub.Application.Features.Benchmarks.GetBenchmarkById;

public record GetBenchmarkByIdQuery(Guid Id)
    : IRequest<BenchmarkDto?>;

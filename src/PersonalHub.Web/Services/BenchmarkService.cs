using PersonalHub.Application.Features.Benchmarks.Common;
using PersonalHub.Application.Features.Benchmarks.CreateBenchmark;
using PersonalHub.Application.Features.Benchmarks.UpdateBenchmark;

namespace PersonalHub.Web.Services;

public class BenchmarkService : BaseHttpService
{
    public BenchmarkService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<BenchmarkDto>> GetBenchmarksAsync()
    {
        return await GetAllAsync<BenchmarkDto>("api/benchmarks");
    }

    public async Task<BenchmarkDto?> GetBenchmarkByIdAsync(Guid id)
    {
        return await GetByIdAsync<BenchmarkDto>($"api/benchmarks/{id}");
    }

    public async Task<Guid> CreateBenchmarkAsync(CreateBenchmarkCommand command)
    {
        return await CreateAsync("api/benchmarks", command);
    }

    public async Task UpdateBenchmarkAsync(Guid id, UpdateBenchmarkCommand command)
    {
        await UpdateAsync($"api/benchmarks/{id}", command);
    }

    public async Task DeleteBenchmarkAsync(Guid id)
    {
        await DeleteAsync($"api/benchmarks/{id}");
    }
}

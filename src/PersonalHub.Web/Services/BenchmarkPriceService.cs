using PersonalHub.Application.Features.BenchmarkPrices.Common;
using PersonalHub.Application.Features.BenchmarkPrices.CreateBenchmarkPrice;
using PersonalHub.Application.Features.BenchmarkPrices.UpdateBenchmarkPrice;

namespace PersonalHub.Web.Services;

public class BenchmarkPriceService : BaseHttpService
{
    public BenchmarkPriceService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<BenchmarkPriceDto>> GetBenchmarkPricesAsync(Guid benchmarkId)
    {
        return await GetAllAsync<BenchmarkPriceDto>($"api/benchmark-prices?benchmarkId={benchmarkId}");
    }

    public async Task<Guid> CreateBenchmarkPriceAsync(CreateBenchmarkPriceCommand command)
    {
        return await CreateAsync("api/benchmark-prices", command);
    }

    public async Task UpdateBenchmarkPriceAsync(Guid id, UpdateBenchmarkPriceCommand command)
    {
        await UpdateAsync($"api/benchmark-prices/{id}", command);
    }

    public async Task DeleteBenchmarkPriceAsync(Guid id)
    {
        await DeleteAsync($"api/benchmark-prices/{id}");
    }
}

using PersonalHub.Application.Features.ExchangeRates.Common;
using PersonalHub.Application.Features.ExchangeRates.CreateExchangeRate;
using PersonalHub.Application.Features.ExchangeRates.UpdateExchangeRate;

namespace PersonalHub.Web.Services;

public class ExchangeRateService : BaseHttpService
{
    public ExchangeRateService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<ExchangeRateDto>> GetExchangeRatesAsync(
        Guid? fromCurrencyId = null,
        Guid? toCurrencyId = null,
        DateTime? dateFrom = null,
        DateTime? dateTo = null)
    {
        var queryParams = new List<string>();

        if (fromCurrencyId.HasValue)
            queryParams.Add($"fromCurrencyId={fromCurrencyId.Value}");

        if (toCurrencyId.HasValue)
            queryParams.Add($"toCurrencyId={toCurrencyId.Value}");

        if (dateFrom.HasValue)
            queryParams.Add($"dateFrom={dateFrom.Value:yyyy-MM-dd}");

        if (dateTo.HasValue)
            queryParams.Add($"dateTo={dateTo.Value:yyyy-MM-dd}");

        var endpoint = queryParams.Count > 0
            ? $"api/exchangerates?{string.Join("&", queryParams)}"
            : "api/exchangerates";

        return await GetAllAsync<ExchangeRateDto>(endpoint);
    }

    public async Task<ExchangeRateDto?> GetExchangeRateByIdAsync(Guid id)
    {
        return await GetByIdAsync<ExchangeRateDto>($"api/exchangerates/{id}");
    }

    public async Task<Guid> CreateExchangeRateAsync(CreateExchangeRateCommand command)
    {
        return await CreateAsync("api/exchangerates", command);
    }

    public async Task UpdateExchangeRateAsync(Guid id, UpdateExchangeRateCommand command)
    {
        await UpdateAsync($"api/exchangerates/{id}", command);
    }

    public async Task DeleteExchangeRateAsync(Guid id)
    {
        await DeleteAsync($"api/exchangerates/{id}");
    }
}

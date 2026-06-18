using PersonalHub.Application.Features.Currency.Common;
using PersonalHub.Application.Features.Currency.CreateCurrency;
using PersonalHub.Application.Features.Currency.UpdateCurrency;

namespace PersonalHub.Web.Services;

public class CurrencyService : BaseHttpService
{
    public CurrencyService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<CurrencyDto>> GetCurrenciesAsync()
    {
        return await GetAllAsync<CurrencyDto>("api/currencies");
    }

    public async Task<CurrencyDto?> GetByIdAsync(Guid id)
    {
        return await GetByIdAsync<CurrencyDto>($"api/currencies/{id}");
    }

    public async Task CreateCurrencyAsync(CreateCurrencyCommand command)
    {
        await CreateAsync("api/currencies", command);
    }

    public async Task UpdateCurrencyAsync(Guid id, UpdateCurrencyCommand command)
    {
        await UpdateAsync($"api/currencies/{id}", command);
    }

    public async Task DeleteCurrencyAsync(Guid id)
    {
        await DeleteAsync($"api/currencies/{id}");
    }
}
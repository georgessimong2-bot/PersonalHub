using PersonalHub.Application.Features.Funds.Common;
using PersonalHub.Application.Features.Funds.CreateFund;
using PersonalHub.Application.Features.Funds.UpdateFund;

namespace PersonalHub.Web.Services;

public class FundService
{
    private readonly HttpClient _http;

    public FundService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    public async Task<List<FundDto>> GetFundsAsync()
    {
        return await _http.GetFromJsonAsync<List<FundDto>>("api/funds")
               ?? [];
    }

    public async Task<FundDto?> GetFundByIdAsync(Guid id)
    {
        return await _http.GetFromJsonAsync<FundDto>($"api/funds/{id}");
    }

    public async Task CreateFundAsync(CreateFundCommand command)
    {
        await _http.PostAsJsonAsync("api/funds", command);
    }

    public async Task<bool> UpdateFundAsync(
    Guid id,
    UpdateFundCommand command)
    {
        var response = await _http.PutAsJsonAsync(
            $"api/funds/{id}",
            command);

        return response.IsSuccessStatusCode;
    }

    public async Task DeleteFundAsync(Guid id)
    {
        await _http.DeleteAsync($"api/funds/{id}");
    }
}
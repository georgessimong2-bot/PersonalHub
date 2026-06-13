using PersonalHub.Application.Features.FundTypes.Common;
using PersonalHub.Application.Features.FundTypes.CreateFundType;
using PersonalHub.Application.Features.FundTypes.UpdateFundType;

namespace PersonalHub.Web.Services;

public class FundTypeService
{
    private readonly HttpClient _http;

    public FundTypeService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    public async Task<List<FundTypeDto>> GetFundTypesAsync()
    {
        return await _http.GetFromJsonAsync<List<FundTypeDto>>("api/fundtypes")
               ?? [];
    }

    public async Task CreateFundTypeAsync(CreateFundTypeCommand command)
    {
        await _http.PostAsJsonAsync("api/fundtypes", command);
    }

    public async Task<FundTypeDto?> GetFundTypeByIdAsync(Guid id)
    {
        return await _http.GetFromJsonAsync<FundTypeDto>(
            $"api/fundtypes/{id}");
    }

    public async Task UpdateFundTypeAsync(
    Guid id,
    UpdateFundTypeCommand command)
    {
        await _http.PutAsJsonAsync(
            $"api/fundtypes/{id}",
            command);
    }

    public async Task DeleteFundTypeAsync(
       Guid id)
    {
        await _http.DeleteAsync(
            $"api/fundtypes/{id}");
    }
}
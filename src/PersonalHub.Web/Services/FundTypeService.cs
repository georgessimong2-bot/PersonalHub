using PersonalHub.Application.Features.FundTypes.Common;
using PersonalHub.Application.Features.FundTypes.CreateFundType;
using PersonalHub.Application.Features.FundTypes.UpdateFundType;

namespace PersonalHub.Web.Services;

public class FundTypeService : BaseHttpService
{
    public FundTypeService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<FundTypeDto>> GetFundTypesAsync()
    {
        return await GetAllAsync<FundTypeDto>("api/fundtypes");
    }

    public async Task CreateFundTypeAsync(CreateFundTypeCommand command)
    {
        await CreateAsync("api/fundtypes", command);
    }

    public async Task<FundTypeDto?> GetFundTypeByIdAsync(Guid id)
    {
        return await GetByIdAsync<FundTypeDto>($"api/fundtypes/{id}");
    }

    public async Task UpdateFundTypeAsync(Guid id, UpdateFundTypeCommand command)
    {
        await UpdateAsync($"api/fundtypes/{id}", command);
    }

    public async Task DeleteFundTypeAsync(Guid id)
    {
        await DeleteAsync($"api/fundtypes/{id}");
    }
}
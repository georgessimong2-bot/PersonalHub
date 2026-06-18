using PersonalHub.Application.Features.AssetClasses.Common;
using PersonalHub.Application.Features.AssetClasses.CreateAssetClass;
using PersonalHub.Application.Features.AssetClasses.UpdateAssetClass;

namespace PersonalHub.Web.Services;

public class AssetClassService : BaseHttpService
{
    public AssetClassService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<AssetClassDto>> GetAssetClassesAsync()
    {
        return await GetAllAsync<AssetClassDto>("api/asset-classes");
    }

    public async Task<AssetClassDto?> GetAssetClassByIdAsync(Guid id)
    {
        return await GetByIdAsync<AssetClassDto>($"api/asset-classes/{id}");
    }

    public async Task<Guid> CreateAssetClassAsync(CreateAssetClassCommand command)
    {
        return await CreateAsync("api/asset-classes", command);
    }

    public async Task UpdateAssetClassAsync(Guid id, UpdateAssetClassCommand command)
    {
        await UpdateAsync($"api/asset-classes/{id}", command);
    }

    public async Task DeleteAssetClassAsync(Guid id)
    {
        await DeleteAsync($"api/asset-classes/{id}");
    }
}

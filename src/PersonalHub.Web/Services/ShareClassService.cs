using PersonalHub.Application.Features.ShareClasses.Common;
using PersonalHub.Application.Features.ShareClasses.CreateShareClass;
using PersonalHub.Application.Features.ShareClasses.UpdateShareClass;

namespace PersonalHub.Web.Services;

public class ShareClassService : BaseHttpService
{
    public ShareClassService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<ShareClassDto>> GetShareClassesAsync()
    {
        return await GetAllAsync<ShareClassDto>("api/share-classes");
    }

    public async Task<ShareClassDto?> GetShareClassByIdAsync(Guid id)
    {
        return await GetByIdAsync<ShareClassDto>($"api/share-classes/{id}");
    }

    public async Task<Guid> CreateShareClassAsync(CreateShareClassCommand command)
    {
        return await CreateAsync("api/share-classes", command);
    }

    public async Task UpdateShareClassAsync(Guid id, UpdateShareClassCommand command)
    {
        await UpdateAsync($"api/share-classes/{id}", command);
    }

    public async Task DeleteShareClassAsync(Guid id)
    {
        await DeleteAsync($"api/share-classes/{id}");
    }
}

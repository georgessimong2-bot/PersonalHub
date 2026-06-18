using PersonalHub.Application.Features.SfdrClassifications.Common;
using PersonalHub.Application.Features.SfdrClassifications.CreateSfdrClassification;
using PersonalHub.Application.Features.SfdrClassifications.UpdateSfdrClassification;

namespace PersonalHub.Web.Services;

public class SfdrClassificationService : BaseHttpService
{
    public SfdrClassificationService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<SfdrClassificationDto>> GetSfdrClassificationsAsync()
    {
        return await GetAllAsync<SfdrClassificationDto>("api/sfdr-classifications");
    }

    public async Task<SfdrClassificationDto?> GetSfdrClassificationByIdAsync(Guid id)
    {
        return await GetByIdAsync<SfdrClassificationDto>($"api/sfdr-classifications/{id}");
    }

    public async Task<Guid> CreateSfdrClassificationAsync(CreateSfdrClassificationCommand command)
    {
        return await CreateAsync("api/sfdr-classifications", command);
    }

    public async Task UpdateSfdrClassificationAsync(Guid id, UpdateSfdrClassificationCommand command)
    {
        await UpdateAsync($"api/sfdr-classifications/{id}", command);
    }

    public async Task DeleteSfdrClassificationAsync(Guid id)
    {
        await DeleteAsync($"api/sfdr-classifications/{id}");
    }
}

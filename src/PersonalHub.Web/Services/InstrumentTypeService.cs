using PersonalHub.Application.Features.InstrumentTypes.Common;
using PersonalHub.Application.Features.InstrumentTypes.CreateInstrumentType;
using PersonalHub.Application.Features.InstrumentTypes.UpdateInstrumentType;

namespace PersonalHub.Web.Services;

public class InstrumentTypeService : BaseHttpService
{
    public InstrumentTypeService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<InstrumentTypeDto>> GetInstrumentTypesAsync()
    {
        return await GetAllAsync<InstrumentTypeDto>("api/instrument-types");
    }

    public async Task<InstrumentTypeDto?> GetInstrumentTypeByIdAsync(Guid id)
    {
        return await GetByIdAsync<InstrumentTypeDto>($"api/instrument-types/{id}");
    }

    public async Task<Guid> CreateInstrumentTypeAsync(CreateInstrumentTypeCommand command)
    {
        return await CreateAsync("api/instrument-types", command);
    }

    public async Task UpdateInstrumentTypeAsync(Guid id, UpdateInstrumentTypeCommand command)
    {
        await UpdateAsync($"api/instrument-types/{id}", command);
    }

    public async Task DeleteInstrumentTypeAsync(Guid id)
    {
        await DeleteAsync($"api/instrument-types/{id}");
    }
}

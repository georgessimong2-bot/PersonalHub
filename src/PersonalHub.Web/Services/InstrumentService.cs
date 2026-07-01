using PersonalHub.Application.Features.Instruments.Common;
using PersonalHub.Application.Features.Instruments.CreateInstrument;
using PersonalHub.Application.Features.Instruments.UpdateInstrument;

namespace PersonalHub.Web.Services;

public class InstrumentService : BaseHttpService
{
    public InstrumentService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<InstrumentDto>> GetInstrumentsAsync()
    {
        return await GetAllAsync<InstrumentDto>("api/instruments");
    }

    public async Task<InstrumentDto?> GetInstrumentByIdAsync(Guid id)
    {
        return await GetByIdAsync<InstrumentDto>($"api/instruments/{id}");
    }

    public async Task<Guid> CreateInstrumentAsync(CreateInstrumentCommand command)
    {
        return await CreateAsync("api/instruments", command);
    }

    public async Task UpdateInstrumentAsync(Guid id, UpdateInstrumentCommand command)
    {
        await UpdateAsync($"api/instruments/{id}", command);
    }

    public async Task DeleteInstrumentAsync(Guid id)
    {
        await DeleteAsync($"api/instruments/{id}");
    }
}

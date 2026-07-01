using PersonalHub.Application.Features.InstrumentPrices.Common;
using PersonalHub.Application.Features.InstrumentPrices.CreateInstrumentPrice;

namespace PersonalHub.Web.Services;

public class InstrumentPriceService : BaseHttpService
{
    public InstrumentPriceService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<InstrumentPriceDto>> GetInstrumentPricesAsync(Guid instrumentId)
    {
        return await GetAllAsync<InstrumentPriceDto>($"api/instrument-prices?instrumentId={instrumentId}");
    }

    public async Task<Guid> CreateInstrumentPriceAsync(CreateInstrumentPriceCommand command)
    {
        return await CreateAsync("api/instrument-prices", command);
    }

    public async Task DeleteInstrumentPriceAsync(Guid id)
    {
        await DeleteAsync($"api/instrument-prices/{id}");
    }
}

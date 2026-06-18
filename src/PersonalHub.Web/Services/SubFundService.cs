using PersonalHub.Application.Features.SubFunds.Common;
using PersonalHub.Application.Features.SubFunds.CreateSubFund;
using PersonalHub.Application.Features.SubFunds.UpdateSubFund;

namespace PersonalHub.Web.Services;

public class SubFundService : BaseHttpService
{
    public SubFundService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<SubFundDto>> GetSubFundsAsync()
    {
        return await GetAllAsync<SubFundDto>("api/sub-funds");
    }

    public async Task<SubFundDto?> GetSubFundByIdAsync(Guid id)
    {
        return await GetByIdAsync<SubFundDto>($"api/sub-funds/{id}");
    }

    public async Task<Guid> CreateSubFundAsync(CreateSubFundCommand command)
    {
        return await CreateAsync("api/sub-funds", command);
    }

    public async Task UpdateSubFundAsync(Guid id, UpdateSubFundCommand command)
    {
        await UpdateAsync($"api/sub-funds/{id}", command);
    }

    public async Task DeleteSubFundAsync(Guid id)
    {
        await DeleteAsync($"api/sub-funds/{id}");
    }
}

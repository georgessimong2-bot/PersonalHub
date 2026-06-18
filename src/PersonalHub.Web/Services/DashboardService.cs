using PersonalHub.Application.Features.Dashboard.Common;

namespace PersonalHub.Web.Services;

public class DashboardService : BaseHttpService
{
    public DashboardService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<DashboardDto?> GetAsync()
    {
        return await GetAsync<DashboardDto>("api/dashboard");
    }
}
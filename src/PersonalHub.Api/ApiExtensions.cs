using PersonalHub.Api.Endpoints;

namespace PersonalHub.Api;

public static class ApiExtensions
{
    public static void MapEndpoints(
        this WebApplication app)
    {
        app.MapDiagnosticEndpoints();
        app.MapAuthEndpoints();
        app.MapAuthCheckEndpoints();
        app.MapNotesEndpoints();
        app.MapUsersEndpoints();
        app.MapAccountEndpoints();
        app.MapGoalEndpoints();
        app.MapAiEndpoints();
        app.MapFundEndpoints();
        app.MapFundTypeEndpoints();
        app.MapDashboardEndpoints();
        app.MapCurrencyEndpoints();
        app.MapInvestmentStrategiesEndpoints();
        app.MapBenchmarksEndpoints();
        app.MapSfdrClassificationsEndpoints();
        app.MapShareClassesEndpoints();
        app.MapSubFundsEndpoints();
    }
}
using PersonalHub.Api.Endpoints;

namespace PersonalHub.Api;

public static class ApiExtensions
{
    public static void MapEndpoints(
        this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapNotesEndpoints();
        app.MapUsersEndpoints();
        app.MapAccountEndpoints();
        app.MapGoalEndpoints();
        app.MapAiEndpoints();
    }
}
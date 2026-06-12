using PersonalHub.Application.Features.Goals.Common;
using PersonalHub.Application.Features.Goals.CreateGoal;
using PersonalHub.Application.Features.Goals.UpdateGoal;

namespace PersonalHub.Web.Services;

public class GoalService
{
    private readonly HttpClient _http;

    public GoalService(
        IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    public async Task<List<GoalDto>> GetGoalsAsync()
    {
        return await _http.GetFromJsonAsync<List<GoalDto>>(
            "api/goals")
            ?? [];
    }

    public async Task CreateGoalAsync(
        CreateGoalCommand command)
    {
        await _http.PostAsJsonAsync(
            "api/goals",
            command);
    }

    public async Task DeleteGoalAsync(
        Guid id)
    {
        await _http.DeleteAsync(
            $"api/goals/{id}");
    }

    public async Task IncrementGoalAsync(Guid id)
    {
        await _http.PatchAsync(
            $"api/goals/{id}/increment",
            null);
    }

    public async Task<GoalDto?> GetGoalByIdAsync(Guid id)
    {
        return await _http.GetFromJsonAsync<GoalDto>(
            $"api/goals/{id}");
    }

    public async Task UpdateGoalAsync(
    Guid id,
    UpdateGoalCommand command)
    {
        await _http.PutAsJsonAsync(
            $"api/goals/{id}",
            command);
    }

    public async Task<string> GenerateAdviceAsync(Guid goalId)
    {
        var response =
            await _http.PostAsync(
                $"api/ai/goals/{goalId}/advice",
                null);

        Console.WriteLine("STATUS = " + response.StatusCode);

        var content =
            await response.Content.ReadAsStringAsync();

        Console.WriteLine("BODY = " + content);

        response.EnsureSuccessStatusCode();

        return content;
    }
}
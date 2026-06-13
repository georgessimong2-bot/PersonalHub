using PersonalHub.Application.Features.AI;
using PersonalHub.Application.Features.Goals.Common;
using PersonalHub.Application.Features.Goals.CreateGoal;
using PersonalHub.Application.Features.Goals.UpdateGoal;
using PersonalHub.Web.Services.Auth;
using System.Text.Json;

namespace PersonalHub.Web.Services;

public class GoalService
{
    private readonly AuthService _auth;
    private readonly HttpClient _http;

    public GoalService(
     IHttpClientFactory factory,
     AuthService auth)
    {
        _http = factory.CreateClient("Api");
        _auth = auth;
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

    public async Task<bool> UpdateGoalAsync(
    Guid id,
    UpdateGoalCommand command)
    {
        var response = await _http.PutAsJsonAsync(
            $"api/goals/{id}",
            command);
        return response.IsSuccessStatusCode;
    }

    public async Task<AiGoalAdvice?> GenerateAdviceAsync(Guid goalId)
    {
        var token = _auth.GetToken();

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"api/ai/goals/{goalId}/advice");

        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();

        Console.WriteLine("STATUS = " + response.StatusCode);
        Console.WriteLine("BODY = " + content);

        response.EnsureSuccessStatusCode();

        return JsonSerializer.Deserialize<AiGoalAdvice>(
            content,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }
}
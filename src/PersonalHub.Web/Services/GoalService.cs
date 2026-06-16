using PersonalHub.Application.Features.AI;
using PersonalHub.Application.Features.Goals.CreateGoal;
using PersonalHub.Application.Features.Goals.UpdateGoal;
using PersonalHub.Web.Services.Auth;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PersonalHub.Web.Services;

public class GoalService
{
    private readonly HttpClient _http;
    private readonly AuthService _auth;

    public GoalService(
        IHttpClientFactory factory,
        AuthService auth)
    {
        _http = factory.CreateClient("Api");
        _auth = auth;
    }

    private void SetAuthorizationHeader()
    {
        var token = _auth.GetToken();

        Console.WriteLine($"GOAL TOKEN = {token}");

        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
        }
        else
        {
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }

    public async Task<List<GoalDto>> GetGoalsAsync()
    {
        SetAuthorizationHeader();

        return await _http.GetFromJsonAsync<List<GoalDto>>(
            "api/goals")
            ?? [];
    }

    public async Task<GoalDto?> GetGoalByIdAsync(Guid id)
    {
        SetAuthorizationHeader();

        return await _http.GetFromJsonAsync<GoalDto>(
            $"api/goals/{id}");
    }

    public async Task CreateGoalAsync(
        CreateGoalCommand command)
    {
        SetAuthorizationHeader();

        var response = await _http.PostAsJsonAsync(
            "api/goals",
            command);

        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> UpdateGoalAsync(
        Guid id,
        UpdateGoalCommand command)
    {
        SetAuthorizationHeader();

        var response = await _http.PutAsJsonAsync(
            $"api/goals/{id}",
            command);

        return response.IsSuccessStatusCode;
    }

    public async Task DeleteGoalAsync(Guid id)
    {
        SetAuthorizationHeader();

        var response = await _http.DeleteAsync(
            $"api/goals/{id}");

        response.EnsureSuccessStatusCode();
    }

    public async Task IncrementGoalAsync(Guid id)
    {
        SetAuthorizationHeader();

        var response = await _http.PatchAsync(
            $"api/goals/{id}/increment",
            null);

        response.EnsureSuccessStatusCode();
    }

    public async Task<AiGoalAdvice?> GenerateAdviceAsync(Guid goalId)
    {
        SetAuthorizationHeader();

        var response = await _http.PostAsync(
            $"api/ai/goals/{goalId}/advice",
            null);

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
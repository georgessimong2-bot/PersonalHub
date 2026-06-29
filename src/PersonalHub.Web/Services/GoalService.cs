using PersonalHub.Application.Features.AI;
using PersonalHub.Application.Features.Goals.CreateGoal;
using PersonalHub.Application.Features.Goals.UpdateGoal;

namespace PersonalHub.Web.Services;

public class GoalService : BaseHttpService
{
    public GoalService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<GoalDto>> GetGoalsAsync()
    {
        return await GetAllAsync<GoalDto>("api/goals");
    }

    public async Task<GoalDto?> GetGoalByIdAsync(Guid id)
    {
        return await GetByIdAsync<GoalDto>($"api/goals/{id}");
    }

    public async Task CreateGoalAsync(CreateGoalCommand command)
    {
        await CreateAsync("api/goals", command);
    }

    public async Task UpdateGoalAsync(Guid id, UpdateGoalCommand command)
    {
        await UpdateAsync($"api/goals/{id}", command);
    }

    public async Task DeleteGoalAsync(Guid id)
    {
        await DeleteAsync($"api/goals/{id}");
    }

    public async Task IncrementGoalAsync(Guid id)
    {
        var response = await Http.PatchAsync($"api/goals/{id}/increment", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task<AiGoalAdvice?> GenerateAdviceAsync(Guid goalId)
    {
        return await PostAsync<AiGoalAdvice>($"api/ai/goals/{goalId}/advice");
    }

    public async Task SaveGeneratedAdviceAsync(Guid goalId, string generatedAdvice)
    {
        var request = new { GeneratedAdvice = generatedAdvice };
        var response = await Http.PatchAsJsonAsync($"api/goals/{goalId}/advice", request);
        response.EnsureSuccessStatusCode();
    }
}
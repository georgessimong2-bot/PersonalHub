using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.AI.GenerateGoalAdvice;

public class GenerateGoalAdviceHandler
    : IRequestHandler<GenerateGoalAdviceCommand, AiGoalAdvice>
{
    private readonly IAppDbContext _context;
    private readonly IAiService _aiService;

    public GenerateGoalAdviceHandler(
        IAppDbContext context,
        IAiService aiService)
    {
        _context = context;
        _aiService = aiService;
    }

    public async Task<AiGoalAdvice> Handle(
        GenerateGoalAdviceCommand request,
        CancellationToken cancellationToken)
    {
        var goal = await _context.Goals
            .FirstOrDefaultAsync(
                x => x.Id == request.GoalId,
                cancellationToken);

        if (goal is null)
            throw new Exception("Goal not found");

        var percentage =
            goal.TargetValue <= 0
                ? 0
                : Math.Round(
                    (goal.CurrentValue / goal.TargetValue) * 100,
                    1);

        var remaining =
            Math.Max(0, goal.TargetValue - goal.CurrentValue);

        var daysRemaining =
            goal.Deadline.HasValue
                ? (goal.Deadline.Value.Date - DateTime.UtcNow.Date).Days
                : (int?)null;

        var prompt = $"""
You are a productivity coach.

Goal:
{goal.Title}

Description:
{goal.Description}

Target:
{goal.TargetValue}

Current:
{goal.CurrentValue}

Progress:
{percentage}%

Remaining:
{remaining}

Days remaining:
{daysRemaining}

Provide structured coaching advice.

Return ONLY JSON with:
- summary
- keyInsights (array)
- actions (array)
- warning (nullable)
- confidenceScore (0-100)
""";

        Console.WriteLine("PROMPT:");
        Console.WriteLine(prompt);

        var result = await _aiService.GenerateGoalAdviceAsync(prompt);

        Console.WriteLine("AI RESULT:");
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result));

        return result;
    }
}
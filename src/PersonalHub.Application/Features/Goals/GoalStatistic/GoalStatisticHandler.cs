using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Goals.Common;

namespace PersonalHub.Application.Features.Goals.GetGoalStatistics;

public class GetGoalStatisticsHandler
    : IRequestHandler<
        GetGoalStatisticsCommand,
        GoalStatisticsDto>
{
    private readonly IAppDbContext _context;

    public GetGoalStatisticsHandler(
        IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GoalStatisticsDto> Handle(
        GetGoalStatisticsCommand request,
        CancellationToken cancellationToken)
    {
        var goals = await _context.Goals
            .ToListAsync(cancellationToken);

        if (!goals.Any())
        {
            return new GoalStatisticsDto
            {
                TotalGoals = 0,
                CompletedGoals = 0,
                ActiveGoals = 0,
                AverageProgress = 0
            };
        }

        var completedGoals = goals.Count(g =>
            g.CurrentValue >= g.TargetValue);

        var activeGoals = goals.Count(g =>
            g.CurrentValue < g.TargetValue);

        var averageProgress = goals.Average(g =>
        {
            if (g.TargetValue <= 0)
                return 0;

            return ((decimal)g.CurrentValue /
                    g.TargetValue) * 100;
        });

        return new GoalStatisticsDto
        {
            TotalGoals = goals.Count,
            CompletedGoals = completedGoals,
            ActiveGoals = activeGoals,
            AverageProgress =
                Math.Round((decimal)averageProgress, 2)
        };
    }
}
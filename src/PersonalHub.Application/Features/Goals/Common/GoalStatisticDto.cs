namespace PersonalHub.Application.Features.Goals.Common;

public class GoalStatisticsDto
{
    public int TotalGoals { get; set; }

    public int CompletedGoals { get; set; }

    public int ActiveGoals { get; set; }

    public decimal AverageProgress { get; set; }
}
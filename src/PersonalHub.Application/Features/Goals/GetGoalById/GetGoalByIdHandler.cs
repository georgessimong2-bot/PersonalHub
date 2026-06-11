using global::PersonalHub.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Features.Goals.Common;

namespace PersonalHub.Application.Features.Goals.GetGoalById;

public class GetGoalByIdHandler
    : IRequestHandler<
        GetGoalByIdCommand,
        GoalDto?>
{
    private readonly IAppDbContext _context;

    public GetGoalByIdHandler(
        IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GoalDto?> Handle(
        GetGoalByIdCommand request,
        CancellationToken cancellationToken)
    {
        var goal = await _context.Goals
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (goal is null)
            return null;

        return new GoalDto
        {
            Id = goal.Id,
            Title = goal.Title,
            Description = goal.Description,
            TargetValue = goal.TargetValue,
            CurrentValue = goal.CurrentValue,
            Deadline = goal.Deadline,

            ProgressPercentage =
                goal.TargetValue == 0
                ? 0
                : (goal.CurrentValue / goal.TargetValue) * 100,

            Status =
                goal.CurrentValue >= goal.TargetValue
                    ? "Completed"
                    : goal.Deadline.HasValue &&
                      goal.Deadline.Value.Date < DateTime.UtcNow.Date
                        ? "Expired"
                        : "Active",

            DaysRemaining =
                goal.Deadline.HasValue
                    ? (goal.Deadline.Value.Date
                        - DateTime.UtcNow.Date).Days
                    : null
        };
    }
}
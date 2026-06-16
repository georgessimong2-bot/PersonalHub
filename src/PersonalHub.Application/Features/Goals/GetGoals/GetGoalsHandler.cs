using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Goals.GetGoals;

public class GetGoalsHandler
    : IRequestHandler<GetGoalsCommand, List<GoalDto>>
{
    private readonly IAppDbContext _context;
    private readonly IIdentityService _identityService;

    public GetGoalsHandler(
        IAppDbContext context,
        IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<List<GoalDto>> Handle(
     GetGoalsCommand request,
     CancellationToken cancellationToken)
    {
        var goals = await _context.Goals
            .ToListAsync(cancellationToken);

        var result = new List<GoalDto>();

        foreach (var goal in goals)
        {
            var user = await _identityService
                .GetUserByIdAsync(goal.UserId);

            result.Add(new GoalDto
            {
                Id = goal.Id,
                Title = goal.Title,
                Description = goal.Description,
                TargetValue = goal.TargetValue,
                CurrentValue = goal.CurrentValue,
                Deadline = goal.Deadline,

                CreatedBy = user?.Email ?? "Unknown",
                UserId = goal.UserId,

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
                        ? (goal.Deadline.Value.Date - DateTime.UtcNow.Date).Days
                        : null
            });
        }

        return result;
    }
}
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Goals.Common;

namespace PersonalHub.Application.Features.Goals.GetGoals;

public class GetGoalsHandler
    : IRequestHandler<GetGoalsCommand, List<GoalDto>>
{
    private readonly IAppDbContext _context;

    public GetGoalsHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GoalDto>> Handle(
        GetGoalsCommand request,
        CancellationToken cancellationToken)
    {
        return await _context.Goals
            .Select(x => new GoalDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                TargetValue = x.TargetValue,
                CurrentValue = x.CurrentValue,
                Deadline = x.Deadline,

                ProgressPercentage =
                    x.TargetValue == 0
                    ? 0
                    : (x.CurrentValue / x.TargetValue) * 100,

                Status =
                    x.CurrentValue >= x.TargetValue
                        ? "Completed"
                        : x.Deadline.HasValue &&
                          x.Deadline.Value.Date < DateTime.UtcNow.Date
                            ? "Expired"
                            : "Active",

                DaysRemaining =
                    x.Deadline.HasValue
                        ? (x.Deadline.Value.Date - DateTime.UtcNow.Date).Days
                        : null
            })
            .ToListAsync(cancellationToken);
    }
}
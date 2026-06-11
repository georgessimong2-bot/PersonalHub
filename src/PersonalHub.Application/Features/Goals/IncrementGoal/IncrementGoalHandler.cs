using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Goals.IncrementGoal;

public class IncrementGoalHandler
    : IRequestHandler<IncrementGoalCommand>
{
    private readonly IAppDbContext _context;

    public IncrementGoalHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
      IncrementGoalCommand request,
      CancellationToken cancellationToken)
    {
        var goal = await _context.Goals
            .FirstOrDefaultAsync(
                x => x.Id == request.GoalId,
                cancellationToken);

        if (goal is null)
        {
            throw new Exception("Goal not found");
        }

        if (goal.CurrentValue < goal.TargetValue)
        {
            goal.CurrentValue++;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
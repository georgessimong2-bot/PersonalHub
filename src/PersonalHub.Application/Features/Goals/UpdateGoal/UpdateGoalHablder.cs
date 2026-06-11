using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Goals.UpdateGoal;

public class UpdateGoalHandler
    : IRequestHandler<UpdateGoalCommand>
{
    private readonly IAppDbContext _context;

    public UpdateGoalHandler(
        IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateGoalCommand request,
        CancellationToken cancellationToken)
    {
        var goal = await _context.Goals
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (goal is null)
            throw new Exception("Goal not found");

        goal.Title = request.Title;
        goal.Description = request.Description;
        goal.TargetValue = request.TargetValue;
        goal.CurrentValue = request.CurrentValue;
        goal.Deadline = request.Deadline;

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
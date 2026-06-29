using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Goals.UpdateGoal;

public class UpdateGoalHandler
    : IRequestHandler<UpdateGoalCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public UpdateGoalHandler(
        IAppDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(
        UpdateGoalCommand request,
        CancellationToken cancellationToken)
    {
        var goal = await _context.Goals
            .FirstOrDefaultAsync(
                x =>
                    x.Id == request.Id &&
                    x.UserId == _currentUser.UserId,
                cancellationToken);

        if (goal is null)
        {
            throw new Exception(
                "Goal not found or access denied");
        }

        goal.Title = request.Title;
        goal.Description = request.Description;
        goal.TargetValue = request.TargetValue;
        goal.CurrentValue = request.CurrentValue;
        goal.Deadline = request.Deadline;

        if (!string.IsNullOrEmpty(request.GeneratedAdvice))
        {
            goal.GeneratedAdvice = request.GeneratedAdvice;
        }

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
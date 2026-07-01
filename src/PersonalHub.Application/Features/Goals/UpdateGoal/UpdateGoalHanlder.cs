using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalHub.Application.Common.Exceptions;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Goals.UpdateGoal;

public class UpdateGoalHandler
    : IRequestHandler<UpdateGoalCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<UpdateGoalHandler> _logger;

    public UpdateGoalHandler(
        IAppDbContext context,
        ICurrentUserService currentUser,
        ILogger<UpdateGoalHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task Handle(
        UpdateGoalCommand request,
        CancellationToken cancellationToken)
    {
        var isAdmin = _currentUser.IsInRole("ADMIN");
        var userId = _currentUser.UserId;

        _logger.LogInformation(
            "UpdateGoalHandler: GoalId={GoalId}, UserId={UserId}, IsAdmin={IsAdmin}",
            request.Id, userId, isAdmin);

        var goal = await _context.Goals
            .FirstOrDefaultAsync(
                x =>
                    x.Id == request.Id &&
                    (isAdmin || x.UserId == userId),
                cancellationToken);

        if (goal is null)
        {
            _logger.LogWarning(
                "Goal not found: GoalId={GoalId}, UserId={UserId}, IsAdmin={IsAdmin}",
                request.Id, userId, isAdmin);

            throw new BusinessException(
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
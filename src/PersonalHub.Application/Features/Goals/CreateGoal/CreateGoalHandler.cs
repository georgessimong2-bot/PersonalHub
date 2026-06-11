using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.Goals.CreateGoal;

public class CreateGoalHandler
    : IRequestHandler<CreateGoalCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateGoalHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateGoalCommand request,
        CancellationToken cancellationToken)
    {
        var goal = new Goal
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            TargetValue = request.TargetValue,
            CurrentValue = 0,
            CreatedAt = DateTime.UtcNow,
            Deadline = request.Deadline
        };

        _context.Goals.Add(goal);

        await _context.SaveChangesAsync(cancellationToken);

        return goal.Id;
    }
}
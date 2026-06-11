using MediatR;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Goals.DeleteGoal;

public class DeleteGoalHandler
    : IRequestHandler<DeleteGoalCommand>
{
    private readonly IAppDbContext _context;

    public DeleteGoalHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteGoalCommand request,
        CancellationToken cancellationToken)
    {
        var goal = await _context.Goals
            .FindAsync(request.Id);

        if (goal is null)
            throw new Exception("Goal not found");

        _context.Goals.Remove(goal);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
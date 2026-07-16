using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.Goals.DeleteGoal;

public class DeleteGoalHandler
    : IRequestHandler<DeleteGoalCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteGoalHandler(IAppDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(
        DeleteGoalCommand request,
        CancellationToken cancellationToken)
    {
        Goal? goal;
        if (_currentUser.IsInRole("ADMIN"))
        {
            goal = await _context.Goals
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }
        else
        {
            goal = await _context.Goals
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id &&
                         x.UserId == _currentUser.UserId,
                    cancellationToken);
        }

        if (goal is null)
            throw new Exception("Goal not found");

        _context.Goals.Remove(goal);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
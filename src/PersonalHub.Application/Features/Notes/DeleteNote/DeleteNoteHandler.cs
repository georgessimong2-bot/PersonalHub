using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Notes.DeleteNote;

public class DeleteNoteHandler
    : IRequestHandler<DeleteNoteCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteNoteHandler(IAppDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(
        DeleteNoteCommand request,
        CancellationToken cancellationToken)
    {
        var note = await _context.Notes
            .FirstOrDefaultAsync(
                x => x.Id == request.Id &&
                 x.UserId == _currentUser.UserId,
                cancellationToken);

        if (note is null)
        {
            throw new KeyNotFoundException($"Note with id {request.Id} not found");
        }

        _context.Notes.Remove(note);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
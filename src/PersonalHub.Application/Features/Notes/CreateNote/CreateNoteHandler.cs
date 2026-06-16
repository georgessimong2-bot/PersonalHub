using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.Notes.CreateNote;

public class CreateNoteHandler
    : IRequestHandler<CreateNoteCommand, Guid>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateNoteHandler(
        IAppDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        CreateNoteCommand request,
        CancellationToken cancellationToken)
    {
        var note = new Note(
            request.Title,
            request.Content,
            _currentUser.UserId);

        _context.Notes.Add(note);

        await _context.SaveChangesAsync(cancellationToken);

        return note.Id;
    }
}
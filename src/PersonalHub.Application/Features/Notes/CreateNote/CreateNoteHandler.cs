using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Notes.Common;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.Notes.CreateNote;

public class CreateNoteHandler
    : IRequestHandler<CreateNoteCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateNoteHandler(
        IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateNoteCommand request,
        CancellationToken cancellationToken)
    {
        var note = new Note(request.Title, request.Content, string.Empty);

        _context.Notes.Add(note);

        await _context.SaveChangesAsync(cancellationToken);

        return note.Id;
    }
}
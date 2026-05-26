using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Notes.DeleteNote;

public class DeleteNoteHandler
    : IRequestHandler<DeleteNoteCommand>
{
    private readonly IAppDbContext _context;

    public DeleteNoteHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteNoteCommand request,
        CancellationToken cancellationToken)
    {
        var note = await _context.Notes
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
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
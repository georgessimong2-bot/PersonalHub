using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Notes.UpdateNote;

public class UpdateNoteHandler
    : IRequestHandler<UpdateNoteCommand>
{
    private readonly IAppDbContext _context;

    public UpdateNoteHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateNoteCommand request,
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

        note.Update(request.Title, request.Content);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
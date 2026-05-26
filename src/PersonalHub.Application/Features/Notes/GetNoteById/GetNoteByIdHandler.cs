using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Notes.Common;

namespace PersonalHub.Application.Features.Notes.GetNoteById;

public class GetNoteByIdHandler
    : IRequestHandler<GetNoteByIdCommand, NoteDto?>
{
    private readonly IAppDbContext _context;

    public GetNoteByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<NoteDto?> Handle(
        GetNoteByIdCommand request,
        CancellationToken cancellationToken)
    {
        return await _context.Notes
            .Where(x => x.Id == request.Id)
            .Select(x => new NoteDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
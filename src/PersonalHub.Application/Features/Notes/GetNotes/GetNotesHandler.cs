using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Notes.Common;

namespace PersonalHub.Application.Features.Notes.GetNotes;

public class GetNotesHandler
    : IRequestHandler<GetNotesCommand, List<NoteDto>>
{
    private readonly IAppDbContext _context;

    public GetNotesHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<NoteDto>> Handle(
        GetNotesCommand request,
        CancellationToken cancellationToken)
    {
        return await _context.Notes
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new NoteDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

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
            .Select(x => new NoteDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content
            })
            .ToListAsync(cancellationToken);
    }
}
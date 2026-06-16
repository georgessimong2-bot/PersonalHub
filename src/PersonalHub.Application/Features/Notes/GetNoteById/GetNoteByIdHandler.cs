using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Notes.Common;

namespace PersonalHub.Application.Features.Notes.GetNoteById;

public class GetNoteByIdHandler
    : IRequestHandler<GetNoteByIdCommand, NoteDto?>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetNoteByIdHandler(
        IAppDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<NoteDto?> Handle(
        GetNoteByIdCommand request,
        CancellationToken cancellationToken)
    {
        return await _context.Notes
            .Where(x =>
                x.Id == request.Id &&
                x.UserId == _currentUser.UserId)
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
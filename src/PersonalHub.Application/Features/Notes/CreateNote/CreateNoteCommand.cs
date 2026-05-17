using MediatR;

namespace PersonalHub.Application.Features.Notes.CreateNote;

public record CreateNoteCommand(
    string Title,
    string Content
) : IRequest<Guid>;
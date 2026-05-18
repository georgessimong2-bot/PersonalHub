using FluentValidation;

namespace PersonalHub.Application.Features.Notes.CreateNote;

public class CreateNoteValidator
    : AbstractValidator<CreateNoteCommand>
{
    public CreateNoteValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty();
    }
}
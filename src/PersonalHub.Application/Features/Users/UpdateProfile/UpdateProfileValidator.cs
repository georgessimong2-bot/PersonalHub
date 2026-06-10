using FluentValidation;
using PersonalHub.Application.Features.Account.UpdateProfile;

namespace PersonalHub.Application.Features.Users.UpdateProfile;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Le prénom est obligatoire")
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Le nom est obligatoire")
            .MaximumLength(50);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20);

        RuleFor(x => x.Address)
            .MaximumLength(200);
    }
}
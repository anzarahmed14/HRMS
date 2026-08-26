using FluentValidation;

namespace HRMS.Application.Features.EmergencyContacts.Commands.CreateEmergencyContact;

public class CreateEmergencyContactCommandValidator
    : AbstractValidator<CreateEmergencyContactCommand>
{
    public CreateEmergencyContactCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.RelationshipId)
            .NotEmpty();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.AlternatePhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.AlternatePhoneNumber));

        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}

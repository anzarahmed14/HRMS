using FluentValidation;

namespace HRMS.Application.Features.EmergencyContacts.Commands.DeleteEmergencyContact;

public class DeleteEmergencyContactCommandValidator
    : AbstractValidator<DeleteEmergencyContactCommand>
{
    public DeleteEmergencyContactCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

using FluentValidation;

namespace HRMS.Application.Features.EmployeeContacts.Commands.UpdateEmployeeContact;

public class UpdateEmployeeContactCommandValidator
    : AbstractValidator<UpdateEmployeeContactCommand>
{
    public UpdateEmployeeContactCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.ContactType)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.ContactValue)
            .NotEmpty()
            .MaximumLength(200);
    }
}

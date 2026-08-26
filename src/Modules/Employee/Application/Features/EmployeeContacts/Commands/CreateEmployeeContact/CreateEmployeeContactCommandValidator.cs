using FluentValidation;

namespace HRMS.Application.Features.EmployeeContacts.Commands.CreateEmployeeContact;

public class CreateEmployeeContactCommandValidator
    : AbstractValidator<CreateEmployeeContactCommand>
{
    public CreateEmployeeContactCommandValidator()
    {
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

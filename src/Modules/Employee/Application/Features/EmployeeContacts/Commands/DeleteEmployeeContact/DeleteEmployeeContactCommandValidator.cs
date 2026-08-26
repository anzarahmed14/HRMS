using FluentValidation;

namespace HRMS.Application.Features.EmployeeContacts.Commands.DeleteEmployeeContact;

public class DeleteEmployeeContactCommandValidator
    : AbstractValidator<DeleteEmployeeContactCommand>
{
    public DeleteEmployeeContactCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

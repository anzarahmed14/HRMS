using FluentValidation;

namespace HRMS.Application.Features.EmployeeAddresses.Commands.DeleteEmployeeAddress;

public class DeleteEmployeeAddressCommandValidator
    : AbstractValidator<DeleteEmployeeAddressCommand>
{
    public DeleteEmployeeAddressCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

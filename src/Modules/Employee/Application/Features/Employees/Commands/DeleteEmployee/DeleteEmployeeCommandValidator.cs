using FluentValidation;

namespace HRMS.Application.Features.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandValidator
    : AbstractValidator<DeleteEmployeeCommand>
{
    public DeleteEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
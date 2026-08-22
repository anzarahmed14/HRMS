using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Commands.DeleteEmployeeShiftAssignment;

public sealed class DeleteEmployeeShiftAssignmentCommandValidator
    : AbstractValidator<DeleteEmployeeShiftAssignmentCommand>
{
    public DeleteEmployeeShiftAssignmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Employee shift assignment ID is required.");
    }
}

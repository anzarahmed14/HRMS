using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveTypes.Commands.DeleteLeaveType;

public class DeleteLeaveTypeCommandValidator
    : AbstractValidator<DeleteLeaveTypeCommand>
{
    public DeleteLeaveTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

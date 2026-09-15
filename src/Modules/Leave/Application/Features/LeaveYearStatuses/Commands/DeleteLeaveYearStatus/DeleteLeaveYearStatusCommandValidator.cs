using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Commands.DeleteLeaveYearStatus;

public class DeleteLeaveYearStatusCommandValidator
    : AbstractValidator<DeleteLeaveYearStatusCommand>
{
    public DeleteLeaveYearStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

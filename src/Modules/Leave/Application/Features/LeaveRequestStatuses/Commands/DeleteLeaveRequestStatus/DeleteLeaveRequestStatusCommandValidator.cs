using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Commands.DeleteLeaveRequestStatus;

public class DeleteLeaveRequestStatusCommandValidator
    : AbstractValidator<DeleteLeaveRequestStatusCommand>
{
    public DeleteLeaveRequestStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

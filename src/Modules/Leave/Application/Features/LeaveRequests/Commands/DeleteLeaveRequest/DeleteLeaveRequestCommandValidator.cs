using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.DeleteLeaveRequest;

public sealed class DeleteLeaveRequestCommandValidator
    : AbstractValidator<DeleteLeaveRequestCommand>
{
    public DeleteLeaveRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Leave request ID is required.");
    }
}

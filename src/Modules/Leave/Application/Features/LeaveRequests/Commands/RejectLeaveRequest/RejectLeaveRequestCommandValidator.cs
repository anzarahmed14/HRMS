using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.RejectLeaveRequest;

public sealed class RejectLeaveRequestCommandValidator
    : AbstractValidator<RejectLeaveRequestCommand>
{
    public RejectLeaveRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Leave request ID is required.");

        RuleFor(x => x.RejectionReason)
            .NotEmpty()
            .WithMessage("Rejection reason is required.")
            .MaximumLength(500)
            .WithMessage("Rejection reason cannot exceed 500 characters.");
    }
}

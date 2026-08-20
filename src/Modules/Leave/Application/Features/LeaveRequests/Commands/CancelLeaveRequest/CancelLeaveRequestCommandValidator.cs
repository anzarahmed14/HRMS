using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.CancelLeaveRequest;

public sealed class CancelLeaveRequestCommandValidator
    : AbstractValidator<CancelLeaveRequestCommand>
{
    public CancelLeaveRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Leave request ID is required.");

        RuleFor(x => x.CancellationReason)
            .NotEmpty()
            .WithMessage("Cancellation reason is required.")
            .MaximumLength(500)
            .WithMessage("Cancellation reason cannot exceed 500 characters.");
    }
}

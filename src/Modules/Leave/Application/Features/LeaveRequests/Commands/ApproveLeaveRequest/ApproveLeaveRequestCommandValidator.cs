using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.ApproveLeaveRequest;

public sealed class ApproveLeaveRequestCommandValidator
    : AbstractValidator<ApproveLeaveRequestCommand>
{
    public ApproveLeaveRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Leave request ID is required.");

        RuleFor(x => x.ApprovalReason)
            .MaximumLength(500)
            .WithMessage("Approval reason cannot exceed 500 characters.");
    }
}

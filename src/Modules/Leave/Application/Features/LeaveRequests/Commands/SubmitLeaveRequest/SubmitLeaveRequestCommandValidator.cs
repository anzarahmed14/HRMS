using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.SubmitLeaveRequest;

public sealed class SubmitLeaveRequestCommandValidator
    : AbstractValidator<SubmitLeaveRequestCommand>
{
    public SubmitLeaveRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Leave request ID is required.");
    }
}

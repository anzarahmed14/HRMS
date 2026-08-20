using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.UpdateLeaveRequest;

public sealed class UpdateLeaveRequestCommandValidator
    : AbstractValidator<UpdateLeaveRequestCommand>
{
    public UpdateLeaveRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Leave request ID is required.");

        RuleFor(x => x.LeaveTypeId)
            .NotEmpty()
            .WithMessage("Leave type is required.");

        RuleFor(x => x.StartDayPartId)
            .NotEmpty()
            .WithMessage("Start day part is required.");

        RuleFor(x => x.EndDayPartId)
            .NotEmpty()
            .WithMessage("End day part is required.");

        RuleFor(x => x.FromDate)
            .NotEmpty()
            .WithMessage("From date is required.");

        RuleFor(x => x.ToDate)
            .NotEmpty()
            .WithMessage("To date is required.");

        RuleFor(x => x)
            .Must(x => x.FromDate <= x.ToDate)
            .WithMessage("From date cannot be greater than to date.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Reason is required.")
            .MaximumLength(500)
            .WithMessage("Reason cannot exceed 500 characters.");
    }
}

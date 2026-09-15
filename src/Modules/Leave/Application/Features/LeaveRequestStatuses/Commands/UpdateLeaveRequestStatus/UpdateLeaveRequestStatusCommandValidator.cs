using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Commands.UpdateLeaveRequestStatus;

public class UpdateLeaveRequestStatusCommandValidator
    : AbstractValidator<UpdateLeaveRequestStatusCommand>
{
    public UpdateLeaveRequestStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}

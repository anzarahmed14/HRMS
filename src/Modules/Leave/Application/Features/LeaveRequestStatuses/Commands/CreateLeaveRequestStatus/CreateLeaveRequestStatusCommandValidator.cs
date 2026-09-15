using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Commands.CreateLeaveRequestStatus;

public class CreateLeaveRequestStatusCommandValidator
    : AbstractValidator<CreateLeaveRequestStatusCommand>
{
    public CreateLeaveRequestStatusCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}

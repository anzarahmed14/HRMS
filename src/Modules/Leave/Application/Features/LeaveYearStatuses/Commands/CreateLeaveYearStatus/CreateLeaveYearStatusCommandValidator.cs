using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Commands.CreateLeaveYearStatus;

public class CreateLeaveYearStatusCommandValidator
    : AbstractValidator<CreateLeaveYearStatusCommand>
{
    public CreateLeaveYearStatusCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}

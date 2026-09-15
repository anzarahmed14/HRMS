using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Commands.UpdateLeaveYearStatus;

public class UpdateLeaveYearStatusCommandValidator
    : AbstractValidator<UpdateLeaveYearStatusCommand>
{
    public UpdateLeaveYearStatusCommandValidator()
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

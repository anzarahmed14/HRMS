using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveYears.Commands.DeleteLeaveYear;

public class DeleteLeaveYearCommandValidator
    : AbstractValidator<DeleteLeaveYearCommand>
{
    public DeleteLeaveYearCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

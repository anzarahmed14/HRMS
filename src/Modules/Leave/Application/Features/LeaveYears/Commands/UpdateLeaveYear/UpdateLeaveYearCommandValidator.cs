using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveYears.Commands.UpdateLeaveYear;

public class UpdateLeaveYearCommandValidator
    : AbstractValidator<UpdateLeaveYearCommand>
{
    public UpdateLeaveYearCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate);

        RuleFor(x => x.StatusId)
            .NotEmpty();
    }
}

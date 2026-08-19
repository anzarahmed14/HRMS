using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveYears.Commands.CreateLeaveYear;

public class CreateLeaveYearCommandValidator
    : AbstractValidator<CreateLeaveYearCommand>
{
    public CreateLeaveYearCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThan(x => x.StartDate);

        RuleFor(x => x.StatusId)
            .NotEmpty();
    }
}
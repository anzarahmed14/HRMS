using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.Commands.CreateLeaveDayPart;

public class CreateLeaveDayPartCommandValidator
    : AbstractValidator<CreateLeaveDayPartCommand>
{
    public CreateLeaveDayPartCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.DaysValue)
            .NotEmpty()
            .PrecisionScale(4, 2, false);
    }
}

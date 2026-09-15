using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.Commands.UpdateLeaveDayPart;

public class UpdateLeaveDayPartCommandValidator
    : AbstractValidator<UpdateLeaveDayPartCommand>
{
    public UpdateLeaveDayPartCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

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

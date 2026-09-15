using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.Commands.DeleteLeaveDayPart;

public class DeleteLeaveDayPartCommandValidator
    : AbstractValidator<DeleteLeaveDayPartCommand>
{
    public DeleteLeaveDayPartCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

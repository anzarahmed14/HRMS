using FluentValidation;

namespace HRMS.Application.Features.Nominees.Commands.UpdateEmployeeNominee;

public class UpdateEmployeeNomineeCommandValidator
    : AbstractValidator<UpdateEmployeeNomineeCommand>
{
    public UpdateEmployeeNomineeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.RelationshipId)
            .NotEmpty();

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.IsMinor)
            .Equal(true)
            .When(x => x.DateOfBirth > DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
            .WithMessage("Nominee under 18 must be marked as minor.");

        RuleFor(x => x.IsMinor)
            .Equal(false)
            .When(x => x.DateOfBirth <= DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
            .WithMessage("Nominee aged 18 or above cannot be marked as minor.");
    }
}

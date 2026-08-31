using FluentValidation;

namespace HRMS.Application.Features.Dependents.Commands.CreateEmployeeDependent;

public class CreateEmployeeDependentCommandValidator
    : AbstractValidator<CreateEmployeeDependentCommand>
{
    public CreateEmployeeDependentCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.RelationshipId)
            .NotEmpty();

        RuleFor(x => x.GenderId)
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
    }
}

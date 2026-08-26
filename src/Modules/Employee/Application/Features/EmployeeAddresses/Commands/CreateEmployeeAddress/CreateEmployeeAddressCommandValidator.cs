using FluentValidation;

namespace HRMS.Application.Features.EmployeeAddresses.Commands.CreateEmployeeAddress;

public class CreateEmployeeAddressCommandValidator
    : AbstractValidator<CreateEmployeeAddressCommand>
{
    public CreateEmployeeAddressCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.AddressTypeId)
            .NotEmpty();

        RuleFor(x => x.CountryId)
            .NotEmpty();

        RuleFor(x => x.StateId)
            .NotEmpty();

        RuleFor(x => x.AddressLine1)
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(x => x.AddressLine2)
            .MaximumLength(250)
            .When(x => !string.IsNullOrWhiteSpace(x.AddressLine2));

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .MaximumLength(20);
    }
}

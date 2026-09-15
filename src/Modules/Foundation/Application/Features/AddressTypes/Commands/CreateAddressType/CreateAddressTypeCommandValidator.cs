using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Commands.CreateAddressType;

public sealed class CreateAddressTypeCommandValidator
    : AbstractValidator<CreateAddressTypeCommand>
{
    public CreateAddressTypeCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}

using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Commands.UpdateAddressType;

public sealed class UpdateAddressTypeCommandValidator
    : AbstractValidator<UpdateAddressTypeCommand>
{
    public UpdateAddressTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

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

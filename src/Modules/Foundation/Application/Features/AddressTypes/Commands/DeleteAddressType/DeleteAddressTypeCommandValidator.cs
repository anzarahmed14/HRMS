using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Commands.DeleteAddressType;

public sealed class DeleteAddressTypeCommandValidator
    : AbstractValidator<DeleteAddressTypeCommand>
{
    public DeleteAddressTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

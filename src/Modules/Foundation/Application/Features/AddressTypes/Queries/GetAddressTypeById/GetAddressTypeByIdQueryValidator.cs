using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Queries.GetAddressTypeById;

public sealed class GetAddressTypeByIdQueryValidator
    : AbstractValidator<GetAddressTypeByIdQuery>
{
    public GetAddressTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

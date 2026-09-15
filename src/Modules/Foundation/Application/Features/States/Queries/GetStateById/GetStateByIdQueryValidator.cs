using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.States.Queries.GetStateById;

public sealed class GetStateByIdQueryValidator
    : AbstractValidator<GetStateByIdQuery>
{
    public GetStateByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

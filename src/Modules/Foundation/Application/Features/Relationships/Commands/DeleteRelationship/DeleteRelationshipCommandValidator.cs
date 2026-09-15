using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Relationships.Commands.DeleteRelationship;

public class DeleteRelationshipCommandValidator
    : AbstractValidator<DeleteRelationshipCommand>
{
    public DeleteRelationshipCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

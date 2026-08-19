using FluentValidation;

namespace HRMS.Modules.Companies.Application.Features.Companies.Commands.CreateCompany;

public class CreateCompanyCommandValidator
    : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.CompanyCode)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}
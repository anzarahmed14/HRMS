using FluentValidation;

namespace HRMS.Modules.Companies.Application.Features.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.CompanyCode)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}

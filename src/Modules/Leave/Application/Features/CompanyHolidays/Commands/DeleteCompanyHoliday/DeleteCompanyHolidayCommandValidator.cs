using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.Commands.DeleteCompanyHoliday;

public sealed class DeleteCompanyHolidayCommandValidator
    : AbstractValidator<DeleteCompanyHolidayCommand>
{
    public DeleteCompanyHolidayCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Company holiday ID is required.");
    }
}

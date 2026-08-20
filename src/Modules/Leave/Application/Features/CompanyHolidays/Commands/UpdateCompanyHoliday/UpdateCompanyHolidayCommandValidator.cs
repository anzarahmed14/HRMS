using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.Commands.UpdateCompanyHoliday;

public sealed class UpdateCompanyHolidayCommandValidator
    : AbstractValidator<UpdateCompanyHolidayCommand>
{
    public UpdateCompanyHolidayCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Company holiday ID is required.");

        RuleFor(x => x.LeaveYearId)
            .NotEmpty()
            .WithMessage("Leave year is required.");

        RuleFor(x => x.HolidayDate)
            .NotEmpty()
            .WithMessage("Holiday date is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.HolidayType)
            .NotEmpty()
            .MaximumLength(50);
    }
}

using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.Commands.CreateCompanyHoliday;

public sealed class CreateCompanyHolidayCommandValidator
    : AbstractValidator<CreateCompanyHolidayCommand>
{
    public CreateCompanyHolidayCommandValidator()
    {
        RuleFor(x => x.LeaveYearId)
            .NotEmpty()
            .WithMessage("Leave year is required.");

        RuleFor(x => x.HolidayDate)
            .NotEmpty()
            .WithMessage("Holiday date is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Holiday name is required.")
            .MaximumLength(200)
            .WithMessage("Holiday name cannot exceed 200 characters.");

        RuleFor(x => x.HolidayType)
            .NotEmpty()
            .WithMessage("Holiday type is required.")
            .MaximumLength(50)
            .WithMessage("Holiday type cannot exceed 50 characters.");
    }
}

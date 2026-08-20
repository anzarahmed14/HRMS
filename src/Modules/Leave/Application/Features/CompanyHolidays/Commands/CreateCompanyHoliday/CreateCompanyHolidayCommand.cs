using MediatR;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.Commands.CreateCompanyHoliday;

public sealed record CreateCompanyHolidayCommand(
    Guid LeaveYearId,
    DateOnly HolidayDate,
    string Name,
    string HolidayType,
    bool IsOptional
) : IRequest<Guid>;

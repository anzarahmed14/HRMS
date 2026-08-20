using MediatR;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.Commands.UpdateCompanyHoliday;

public sealed record UpdateCompanyHolidayCommand(
    Guid Id,
    Guid LeaveYearId,
    DateOnly HolidayDate,
    string Name,
    string HolidayType,
    bool IsOptional,
    bool IsActive
) : IRequest;

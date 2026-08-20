using MediatR;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.Commands.DeleteCompanyHoliday;

public sealed record DeleteCompanyHolidayCommand(
    Guid Id
) : IRequest;

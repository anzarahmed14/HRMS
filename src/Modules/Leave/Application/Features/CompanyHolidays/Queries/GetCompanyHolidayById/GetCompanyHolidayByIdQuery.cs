using HRMS.Modules.Leave.Application.Features.CompanyHolidays.DTOs;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.Queries.GetCompanyHolidayById;

public sealed record GetCompanyHolidayByIdQuery(
    Guid Id
) : IRequest<CompanyHolidayDto>;

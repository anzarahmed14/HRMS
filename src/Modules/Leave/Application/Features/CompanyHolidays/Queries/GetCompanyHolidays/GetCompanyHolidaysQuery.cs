using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Application.Features.CompanyHolidays.DTOs;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.Queries.GetCompanyHolidays;

public sealed record GetCompanyHolidaysQuery(
    PagedRequest Request
) : IRequest<PagedResult<CompanyHolidayDto>>;

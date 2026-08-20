using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Application.Features.CompanyHolidays.DTOs;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.Queries.GetCompanyHolidays;

public sealed class GetCompanyHolidaysQueryHandler
    : IRequestHandler<
        GetCompanyHolidaysQuery,
        PagedResult<CompanyHolidayDto>>
{
    private readonly IReadRepository<CompanyHoliday, Guid> _repository;

    public GetCompanyHolidaysQueryHandler(
        IReadRepository<CompanyHoliday, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<CompanyHolidayDto>> Handle(
        GetCompanyHolidaysQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<CompanyHolidayDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new CompanyHolidayDto
                {
                    Id = x.Id,
                    LeaveYearId = x.LeaveYearId,
                    HolidayDate = x.HolidayDate,
                    Name = x.Name,
                    HolidayType = x.HolidayType,
                    IsOptional = x.IsOptional,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}

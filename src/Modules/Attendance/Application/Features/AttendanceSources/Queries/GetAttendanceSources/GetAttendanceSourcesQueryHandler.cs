using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceSources.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.Queries.GetAttendanceSources;

public sealed class GetAttendanceSourcesQueryHandler
    : IRequestHandler<
        GetAttendanceSourcesQuery,
        PagedResult<AttendanceSourceDto>>
{
    private readonly IReadRepository<AttendanceSource, Guid>
        _repository;

    public GetAttendanceSourcesQueryHandler(
        IReadRepository<AttendanceSource, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<AttendanceSourceDto>> Handle(
        GetAttendanceSourcesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<AttendanceSourceDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new AttendanceSourceDto
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    Code = x.Code,
                    Name = x.Name,
                    SourceType = x.SourceType,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}

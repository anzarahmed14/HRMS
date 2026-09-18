using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Queries.GetAttendanceRegularizationStatuses;

public sealed class GetAttendanceRegularizationStatusesQueryHandler
    : IRequestHandler<
        GetAttendanceRegularizationStatusesQuery,
        PagedResult<AttendanceRegularizationStatusDto>>
{
    private readonly IReadRepository<AttendanceRegularizationStatus, Guid>
        _repository;

    public GetAttendanceRegularizationStatusesQueryHandler(
        IReadRepository<AttendanceRegularizationStatus, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<AttendanceRegularizationStatusDto>> Handle(
        GetAttendanceRegularizationStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<AttendanceRegularizationStatusDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new AttendanceRegularizationStatusDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}

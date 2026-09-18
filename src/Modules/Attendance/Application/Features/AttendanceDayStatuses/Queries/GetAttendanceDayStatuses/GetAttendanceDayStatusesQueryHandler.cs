using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Queries.GetAttendanceDayStatuses;

public sealed class GetAttendanceDayStatusesQueryHandler
    : IRequestHandler<
        GetAttendanceDayStatusesQuery,
        PagedResult<AttendanceDayStatusDto>>
{
    private readonly IReadRepository<AttendanceDayStatus, Guid>
        _repository;

    public GetAttendanceDayStatusesQueryHandler(
        IReadRepository<AttendanceDayStatus, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<AttendanceDayStatusDto>> Handle(
        GetAttendanceDayStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<AttendanceDayStatusDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new AttendanceDayStatusDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
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

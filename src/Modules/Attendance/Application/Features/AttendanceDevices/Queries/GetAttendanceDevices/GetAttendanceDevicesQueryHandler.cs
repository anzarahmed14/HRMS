using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceDevices.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.Queries.GetAttendanceDevices;

public sealed class GetAttendanceDevicesQueryHandler
    : IRequestHandler<
        GetAttendanceDevicesQuery,
        PagedResult<AttendanceDeviceDto>>
{
    private readonly IReadRepository<AttendanceDevice, Guid>
        _repository;

    public GetAttendanceDevicesQueryHandler(
        IReadRepository<AttendanceDevice, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<AttendanceDeviceDto>> Handle(
        GetAttendanceDevicesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<AttendanceDeviceDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new AttendanceDeviceDto
                {
                    Id = x.Id,
                    AttendanceSourceId = x.AttendanceSourceId,
                    Code = x.Code,
                    Name = x.Name,
                    SerialNumber = x.SerialNumber,
                    IpAddress = x.IpAddress,
                    Location = x.Location,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}

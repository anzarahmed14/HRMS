using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Features.AttendanceDevices.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.Queries.GetAttendanceDeviceById;

public sealed class GetAttendanceDeviceByIdQueryHandler
    : IRequestHandler<GetAttendanceDeviceByIdQuery, AttendanceDeviceDto>
{
    private readonly IReadRepository<AttendanceDevice, Guid>
        _repository;

    public GetAttendanceDeviceByIdQueryHandler(
        IReadRepository<AttendanceDevice, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<AttendanceDeviceDto> Handle(
        GetAttendanceDeviceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var device = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (device is null || device.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Device",
                request.Id);
        }

        return new AttendanceDeviceDto
        {
            Id = device.Id,
            AttendanceSourceId = device.AttendanceSourceId,
            Code = device.Code,
            Name = device.Name,
            SerialNumber = device.SerialNumber,
            IpAddress = device.IpAddress,
            Location = device.Location,
            IsActive = device.IsActive
        };
    }
}

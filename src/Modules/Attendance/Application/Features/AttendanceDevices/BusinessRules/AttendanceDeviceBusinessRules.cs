using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.BusinessRules;

public class AttendanceDeviceBusinessRules
{
    private readonly IReadRepository<AttendanceDevice, Guid>
        _deviceReadRepository;

    private readonly IReadRepository<AttendanceSource, Guid>
        _sourceReadRepository;

    public AttendanceDeviceBusinessRules(
        IReadRepository<AttendanceDevice, Guid> deviceReadRepository,
        IReadRepository<AttendanceSource, Guid> sourceReadRepository)
    {
        _deviceReadRepository = deviceReadRepository;
        _sourceReadRepository = sourceReadRepository;
    }

    public async Task EnsureSourceExistsAsync(
        Guid sourceId,
        CancellationToken cancellationToken = default)
    {
        var source = await _sourceReadRepository.GetByIdAsync(
            sourceId,
            cancellationToken);

        if (source is null || source.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Source",
                sourceId);
        }
    }

    public async Task EnsureCodeUniqueAsync(
        Guid sourceId,
        string code,
        Guid? deviceId = null,
        CancellationToken cancellationToken = default)
    {
        var exists = await _deviceReadRepository.AnyAsync(
            x =>
                x.AttendanceSourceId == sourceId &&
                x.Code == code &&
                !x.IsDeleted &&
                (!deviceId.HasValue || x.Id != deviceId.Value),
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Attendance device code already exists for this source.");
        }
    }

    public async Task EnsureSerialNumberUniqueAsync(
        string? serialNumber,
        Guid? deviceId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(serialNumber))
        {
            return;
        }

        var exists = await _deviceReadRepository.AnyAsync(
            x =>
                x.SerialNumber == serialNumber &&
                !x.IsDeleted &&
                (!deviceId.HasValue || x.Id != deviceId.Value),
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Attendance device serial number already exists.");
        }
    }
}

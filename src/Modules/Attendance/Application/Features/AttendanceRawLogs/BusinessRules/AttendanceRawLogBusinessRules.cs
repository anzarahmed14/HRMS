using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using EmployeeEntity =
    HRMS.Modules.Employee.Domain.Entities.Employee;
namespace HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.BusinessRules;

public class AttendanceRawLogBusinessRules
{
    private readonly IReadRepository<AttendanceRawLog, Guid>
        _rawLogReadRepository;

    private readonly IReadRepository<AttendanceDevice, Guid>
        _deviceReadRepository;

    private readonly IReadRepository<EmployeeEntity, Guid>
        _employeeReadRepository;

    public AttendanceRawLogBusinessRules(
        IReadRepository<AttendanceRawLog, Guid> rawLogReadRepository,
        IReadRepository<AttendanceDevice, Guid> deviceReadRepository,
        IReadRepository<EmployeeEntity, Guid> employeeReadRepository)
    {
        _rawLogReadRepository = rawLogReadRepository;
        _deviceReadRepository = deviceReadRepository;
        _employeeReadRepository = employeeReadRepository;
    }

    public async Task EnsureEmployeeExistsAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var employee = await _employeeReadRepository.GetByIdAsync(
            employeeId,
            cancellationToken);

        if (employee is null || employee.IsDeleted)
        {
            throw new NotFoundException(
                "Employee",
                employeeId);
        }
    }

    public async Task EnsureDeviceExistsAsync(
        Guid deviceId,
        CancellationToken cancellationToken = default)
    {
        var device = await _deviceReadRepository.GetByIdAsync(
            deviceId,
            cancellationToken);

        if (device is null || device.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Device",
                deviceId);
        }
    }

    public async Task EnsureExternalRecordIsUniqueAsync(
        string? externalRecordId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(externalRecordId))
        {
            return;
        }

        var exists = await _rawLogReadRepository.AnyAsync(
            x =>
                x.ExternalRecordId == externalRecordId &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Attendance raw log has already been imported.");
        }
    }
}

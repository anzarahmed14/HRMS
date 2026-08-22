using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Features.AttendanceDevices.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.Commands.UpdateAttendanceDevice;

public sealed class UpdateAttendanceDeviceCommandHandler
    : IRequestHandler<UpdateAttendanceDeviceCommand>
{
    private readonly IReadRepository<AttendanceDevice, Guid>
        _readRepository;

    private readonly IWriteRepository<AttendanceDevice, Guid>
        _writeRepository;

    private readonly AttendanceDeviceBusinessRules
        _businessRules;

    public UpdateAttendanceDeviceCommandHandler(
        IReadRepository<AttendanceDevice, Guid> readRepository,
        IWriteRepository<AttendanceDevice, Guid> writeRepository,
        AttendanceDeviceBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateAttendanceDeviceCommand request,
        CancellationToken cancellationToken)
    {
        var device = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (device is null || device.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Device",
                request.Id);
        }

        var code = request.Code.Trim();

        var serialNumber = string.IsNullOrWhiteSpace(request.SerialNumber)
            ? null
            : request.SerialNumber.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            device.AttendanceSourceId,
            code,
            request.Id,
            cancellationToken);

        await _businessRules.EnsureSerialNumberUniqueAsync(
            serialNumber,
            request.Id,
            cancellationToken);

        device.Code = code;
        device.Name = request.Name.Trim();
        device.SerialNumber = serialNumber;
        device.IpAddress = request.IpAddress?.Trim();
        device.Location = request.Location?.Trim();
        device.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            device,
            cancellationToken);
    }
}

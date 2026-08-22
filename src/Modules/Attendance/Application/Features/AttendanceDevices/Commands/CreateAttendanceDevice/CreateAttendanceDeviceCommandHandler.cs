using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendanceDevices.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.Commands.CreateAttendanceDevice;

public sealed class CreateAttendanceDeviceCommandHandler
    : IRequestHandler<CreateAttendanceDeviceCommand, Guid>
{
    private readonly IWriteRepository<AttendanceDevice, Guid>
        _writeRepository;

    private readonly AttendanceDeviceBusinessRules
        _businessRules;

    public CreateAttendanceDeviceCommandHandler(
        IWriteRepository<AttendanceDevice, Guid> writeRepository,
        AttendanceDeviceBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateAttendanceDeviceCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureSourceExistsAsync(
            request.AttendanceSourceId,
            cancellationToken);

        var code = request.Code.Trim();

        var serialNumber = string.IsNullOrWhiteSpace(request.SerialNumber)
            ? null
            : request.SerialNumber.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            request.AttendanceSourceId,
            code,
            cancellationToken: cancellationToken);

        await _businessRules.EnsureSerialNumberUniqueAsync(
            serialNumber,
            cancellationToken: cancellationToken);

        var device = new AttendanceDevice
        {
            AttendanceSourceId = request.AttendanceSourceId,
            Code = code,
            Name = request.Name.Trim(),
            SerialNumber = serialNumber,
            IpAddress = request.IpAddress?.Trim(),
            Location = request.Location?.Trim(),
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            device,
            cancellationToken);

        return device.Id;
    }
}

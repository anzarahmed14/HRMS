using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.Commands.DeleteAttendanceDevice;

public sealed class DeleteAttendanceDeviceCommandHandler
    : IRequestHandler<DeleteAttendanceDeviceCommand>
{
    private readonly IReadRepository<AttendanceDevice, Guid>
        _readRepository;

    private readonly IWriteRepository<AttendanceDevice, Guid>
        _writeRepository;

    public DeleteAttendanceDeviceCommandHandler(
        IReadRepository<AttendanceDevice, Guid> readRepository,
        IWriteRepository<AttendanceDevice, Guid> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        DeleteAttendanceDeviceCommand request,
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

        await _writeRepository.DeleteAsync(
            device,
            cancellationToken);
    }
}

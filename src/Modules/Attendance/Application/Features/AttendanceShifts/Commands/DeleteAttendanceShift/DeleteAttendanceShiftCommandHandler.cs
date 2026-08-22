using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Commands.DeleteAttendanceShift;

public sealed class DeleteAttendanceShiftCommandHandler
    : IRequestHandler<DeleteAttendanceShiftCommand>
{
    private readonly IReadRepository<AttendanceShift, Guid> _readRepository;
    private readonly IWriteRepository<AttendanceShift, Guid> _writeRepository;

    public DeleteAttendanceShiftCommandHandler(
        IReadRepository<AttendanceShift, Guid> readRepository,
        IWriteRepository<AttendanceShift, Guid> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        DeleteAttendanceShiftCommand request,
        CancellationToken cancellationToken)
    {
        var shift = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (shift is null || shift.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Shift",
                request.Id);
        }

        await _writeRepository.DeleteAsync(
            shift,
            cancellationToken);
    }
}

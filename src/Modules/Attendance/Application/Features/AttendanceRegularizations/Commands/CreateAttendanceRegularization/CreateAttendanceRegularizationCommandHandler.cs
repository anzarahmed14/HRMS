using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.CreateAttendanceRegularization;

public sealed class CreateAttendanceRegularizationCommandHandler
    : IRequestHandler<CreateAttendanceRegularizationCommand, Guid>
{
    private static readonly Guid PendingStatusId =
        Guid.Parse("10000000-0000-0000-0000-000000000101");

    private readonly IReadRepository<AttendanceRecord, Guid>
        _attendanceRecordRepository;

    private readonly IReadRepository<AttendanceRegularization, Guid>
        _regularizationRepository;

    private readonly IWriteRepository<AttendanceRegularization, Guid>
        _writeRepository;

    public CreateAttendanceRegularizationCommandHandler(
        IReadRepository<AttendanceRecord, Guid> attendanceRecordRepository,
        IReadRepository<AttendanceRegularization, Guid> regularizationRepository,
        IWriteRepository<AttendanceRegularization, Guid> writeRepository)
    {
        _attendanceRecordRepository = attendanceRecordRepository;
        _regularizationRepository = regularizationRepository;
        _writeRepository = writeRepository;
    }

    public async Task<Guid> Handle(
        CreateAttendanceRegularizationCommand request,
        CancellationToken cancellationToken)
    {
        var attendanceRecord =
            await _attendanceRecordRepository.FirstOrDefaultAsync(
                x =>
                    x.EmployeeId == request.EmployeeId &&
                    x.AttendanceDate == request.AttendanceDate &&
                    !x.IsDeleted,
                cancellationToken);

        if (attendanceRecord is null)
        {
            throw new NotFoundException(
                "Attendance record",
                request.AttendanceDate);
        }

        var existing =
            await _regularizationRepository.FirstOrDefaultAsync(
                x =>
                    x.EmployeeId == request.EmployeeId &&
                    x.AttendanceDate == request.AttendanceDate &&
                    x.AttendanceRegularizationStatusId ==
                        PendingStatusId &&
                    !x.IsDeleted,
                cancellationToken);

        if (existing is not null)
        {
            throw new ConflictException(
                "A pending attendance regularization already exists for this employee and date.");
        }

        var regularization = new AttendanceRegularization
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            AttendanceRecordId = attendanceRecord.Id,
            AttendanceRegularizationStatusId = PendingStatusId,
            AttendanceDate = request.AttendanceDate,
            RequestedCheckIn = request.RequestedCheckIn,
            RequestedCheckOut = request.RequestedCheckOut,
            Reason = request.Reason,
            RequestedOn = DateTimeOffset.UtcNow
        };

        await _writeRepository.AddAsync(
            regularization,
            cancellationToken);

        return regularization.Id;
    }
}

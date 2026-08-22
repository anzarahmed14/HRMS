using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.Commands.CreateAttendanceRawLog;

public sealed class CreateAttendanceRawLogCommandHandler
    : IRequestHandler<CreateAttendanceRawLogCommand, Guid>
{
    private readonly IWriteRepository<AttendanceRawLog, Guid>
        _writeRepository;

    private readonly AttendanceRawLogBusinessRules
        _businessRules;

    public CreateAttendanceRawLogCommandHandler(
        IWriteRepository<AttendanceRawLog, Guid> writeRepository,
        AttendanceRawLogBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateAttendanceRawLogCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureDeviceExistsAsync(
            request.AttendanceDeviceId,
            cancellationToken);

        var externalRecordId =
            string.IsNullOrWhiteSpace(request.ExternalRecordId)
                ? null
                : request.ExternalRecordId.Trim();

        await _businessRules.EnsureExternalRecordIsUniqueAsync(
            externalRecordId,
            cancellationToken);

        var rawLog = new AttendanceRawLog
        {
            EmployeeId = request.EmployeeId,
            AttendanceDeviceId = request.AttendanceDeviceId,
            PunchDateTime = request.PunchDateTime,
            PunchType = request.PunchType.Trim().ToUpperInvariant(),
            ExternalRecordId = externalRecordId,
            RawData = request.RawData,
            ImportedOn = DateTimeOffset.UtcNow
        };

        await _writeRepository.AddAsync(
            rawLog,
            cancellationToken);

        return rawLog.Id;
    }
}

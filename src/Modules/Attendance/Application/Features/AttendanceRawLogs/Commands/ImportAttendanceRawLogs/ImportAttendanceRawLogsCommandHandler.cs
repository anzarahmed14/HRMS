using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.Commands.ImportAttendanceRawLogs;

public sealed class ImportAttendanceRawLogsCommandHandler
    : IRequestHandler<ImportAttendanceRawLogsCommand, int>
{
    private readonly IWriteRepository<AttendanceRawLog, Guid>
        _writeRepository;

    private readonly AttendanceRawLogBusinessRules
        _businessRules;

    public ImportAttendanceRawLogsCommandHandler(
        IWriteRepository<AttendanceRawLog, Guid> writeRepository,
        AttendanceRawLogBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<int> Handle(
        ImportAttendanceRawLogsCommand request,
        CancellationToken cancellationToken)
    {
        var records = request.Records.ToList();

        var externalIds = records
            .Where(x => !string.IsNullOrWhiteSpace(x.ExternalRecordId))
            .Select(x => x.ExternalRecordId!.Trim())
            .ToList();

        var duplicateExternalIds = externalIds
            .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToList();

        if (duplicateExternalIds.Count > 0)
        {
            throw new HRMS.BuildingBlocks.Application.Exceptions.ConflictException(
                $"Duplicate external record IDs found in import: {string.Join(", ", duplicateExternalIds)}");
        }

        var employees = records
            .Select(x => x.EmployeeId)
            .Distinct()
            .ToList();

        foreach (var employeeId in employees)
        {
            await _businessRules.EnsureEmployeeExistsAsync(
                employeeId,
                cancellationToken);
        }

        var devices = records
            .Select(x => x.AttendanceDeviceId)
            .Distinct()
            .ToList();

        foreach (var deviceId in devices)
        {
            await _businessRules.EnsureDeviceExistsAsync(
                deviceId,
                cancellationToken);
        }

        foreach (var record in records)
        {
            var externalRecordId =
                string.IsNullOrWhiteSpace(record.ExternalRecordId)
                    ? null
                    : record.ExternalRecordId.Trim();

            await _businessRules.EnsureExternalRecordIsUniqueAsync(
                externalRecordId,
                cancellationToken);

            var rawLog = new AttendanceRawLog
            {
                Id = Guid.NewGuid(),
                EmployeeId = record.EmployeeId,
                AttendanceDeviceId = record.AttendanceDeviceId,
                PunchDateTime = record.PunchDateTime,
                PunchType = record.PunchType.Trim().ToUpperInvariant(),
                ExternalRecordId = externalRecordId,
                RawData = record.RawData,
                ImportedOn = DateTimeOffset.UtcNow
            };

            await _writeRepository.AddAsync(
                rawLog,
                cancellationToken);
        }

        return records.Count;
    }
}

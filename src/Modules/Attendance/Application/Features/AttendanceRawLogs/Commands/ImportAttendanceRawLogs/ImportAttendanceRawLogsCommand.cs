using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.Commands.ImportAttendanceRawLogs;

public sealed record ImportAttendanceRawLogItem(
    Guid EmployeeId,
    Guid AttendanceDeviceId,
    DateTimeOffset PunchDateTime,
    string PunchType,
    string? ExternalRecordId,
    string? RawData
);

public sealed record ImportAttendanceRawLogsCommand(
    IReadOnlyCollection<ImportAttendanceRawLogItem> Records
) : IRequest<int>;

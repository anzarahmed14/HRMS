using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.Commands.CreateAttendanceRawLog;

public sealed record CreateAttendanceRawLogCommand(
    Guid EmployeeId,
    Guid AttendanceDeviceId,
    DateTimeOffset PunchDateTime,
    string PunchType,
    string? ExternalRecordId,
    string? RawData
) : IRequest<Guid>;

using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Commands.CreateManualAttendanceRecord;

public sealed record CreateManualAttendanceRecordCommand(
    Guid EmployeeId,
    DateOnly AttendanceDate,
    DateTimeOffset? CheckIn,
    DateTimeOffset? CheckOut,
    string? Remarks
) : IRequest<Guid>;

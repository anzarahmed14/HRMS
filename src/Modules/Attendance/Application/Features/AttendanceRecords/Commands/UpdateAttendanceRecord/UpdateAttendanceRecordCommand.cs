using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Commands.UpdateAttendanceRecord;

public sealed record UpdateAttendanceRecordCommand(
    Guid Id,
    DateTimeOffset? CheckIn,
    DateTimeOffset? CheckOut,
    string? Remarks
) : IRequest;

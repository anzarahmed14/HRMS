using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Commands.ProcessAttendanceRecords;

public sealed record ProcessAttendanceRecordsCommand(
    Guid EmployeeId,
    DateOnly FromDate,
    DateOnly ToDate
) : IRequest<int>;

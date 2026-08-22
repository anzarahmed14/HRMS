using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Commands.DeleteAttendanceShift;

public sealed record DeleteAttendanceShiftCommand(
    Guid Id
) : IRequest;

using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Commands.UpdateAttendanceShift;

public sealed record UpdateAttendanceShiftCommand(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int BreakMinutes,
    bool IsOvernight,
    bool IsActive,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo
) : IRequest;

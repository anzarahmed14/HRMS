using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Commands.CreateAttendanceShift;

public sealed record CreateAttendanceShiftCommand(
    Guid CompanyId,
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
) : IRequest<Guid>;

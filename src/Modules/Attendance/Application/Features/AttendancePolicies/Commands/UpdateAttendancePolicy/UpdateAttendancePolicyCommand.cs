using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Commands.UpdateAttendancePolicy;

public sealed record UpdateAttendancePolicyCommand(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    int GracePeriodMinutes,
    int MinimumWorkingMinutes,
    int FullDayMinutes,
    int HalfDayMinutes,
    bool IsOvertimeAllowed,
    int MinimumOvertimeMinutes,
    int MaximumOvertimeMinutes,
    bool OvertimeRequiresApproval,
    bool IsDefault,
    bool IsActive,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo
) : IRequest;

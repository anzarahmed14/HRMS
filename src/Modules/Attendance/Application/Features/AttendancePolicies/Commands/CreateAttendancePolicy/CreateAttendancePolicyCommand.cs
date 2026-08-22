using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Commands.CreateAttendancePolicy;

public record CreateAttendancePolicyCommand : IRequest<Guid>
{
    public Guid CompanyId { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int GracePeriodMinutes { get; init; }

    public int MinimumWorkingMinutes { get; init; }

    public int FullDayMinutes { get; init; }

    public int HalfDayMinutes { get; init; }

    public bool IsOvertimeAllowed { get; init; }

    public int MinimumOvertimeMinutes { get; init; }

    public int MaximumOvertimeMinutes { get; init; }

    public bool OvertimeRequiresApproval { get; init; }

    public bool IsDefault { get; init; }

    public bool IsActive { get; init; }

    public DateOnly EffectiveFrom { get; init; }

    public DateOnly? EffectiveTo { get; init; }
}

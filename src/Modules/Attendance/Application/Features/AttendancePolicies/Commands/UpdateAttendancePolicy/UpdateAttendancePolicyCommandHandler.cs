using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Commands.UpdateAttendancePolicy;

public class UpdateAttendancePolicyCommandHandler
    : IRequestHandler<UpdateAttendancePolicyCommand>
{
    private readonly IReadRepository<AttendancePolicy, Guid>
        _attendancePolicyReadRepository;

    private readonly IWriteRepository<AttendancePolicy, Guid>
        _attendancePolicyWriteRepository;

    private readonly AttendancePolicyBusinessRules
        _attendancePolicyRules;

    public UpdateAttendancePolicyCommandHandler(
        IReadRepository<AttendancePolicy, Guid> attendancePolicyReadRepository,
        IWriteRepository<AttendancePolicy, Guid> attendancePolicyWriteRepository,
        AttendancePolicyBusinessRules attendancePolicyRules)
    {
        _attendancePolicyReadRepository = attendancePolicyReadRepository;
        _attendancePolicyWriteRepository = attendancePolicyWriteRepository;
        _attendancePolicyRules = attendancePolicyRules;
    }

    public async Task Handle(
        UpdateAttendancePolicyCommand request,
        CancellationToken cancellationToken)
    {
        var attendancePolicy =
            await _attendancePolicyReadRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

        if (attendancePolicy is null || attendancePolicy.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Policy",
                request.Id);
        }

        await _attendancePolicyRules.EnsurePolicyCodeUniqueAsync(
     attendancePolicy.CompanyId,
     request.Code,
     request.Id,
     cancellationToken);

        if (request.IsDefault)
        {
            await _attendancePolicyRules.EnsureDefaultPolicyUniqueAsync(
                attendancePolicy.CompanyId,
                request.Id,
                cancellationToken);
        }

        attendancePolicy.Code = request.Code.Trim();

        attendancePolicy.Name = request.Name.Trim();

        attendancePolicy.Description =
            request.Description?.Trim();

        attendancePolicy.GracePeriodMinutes =
            request.GracePeriodMinutes;

        attendancePolicy.MinimumWorkingMinutes =
            request.MinimumWorkingMinutes;

        attendancePolicy.FullDayMinutes =
            request.FullDayMinutes;

        attendancePolicy.HalfDayMinutes =
            request.HalfDayMinutes;

        attendancePolicy.IsOvertimeAllowed =
            request.IsOvertimeAllowed;

        attendancePolicy.MinimumOvertimeMinutes =
            request.MinimumOvertimeMinutes;

        attendancePolicy.MaximumOvertimeMinutes =
            request.MaximumOvertimeMinutes;

        attendancePolicy.OvertimeRequiresApproval =
            request.OvertimeRequiresApproval;

        attendancePolicy.IsDefault =
            request.IsDefault;

        attendancePolicy.IsActive =
            request.IsActive;

        attendancePolicy.EffectiveFrom =
            request.EffectiveFrom;

        attendancePolicy.EffectiveTo =
            request.EffectiveTo;

        await _attendancePolicyWriteRepository.UpdateAsync(
            attendancePolicy,
            cancellationToken);
    }
}
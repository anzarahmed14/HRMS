using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Commands.UpdateAttendanceRecord;

public sealed class UpdateAttendanceRecordCommandHandler
    : IRequestHandler<UpdateAttendanceRecordCommand>
{
    private readonly IReadRepository<AttendanceRecord, Guid>
        _recordRepository;

    private readonly IReadRepository<EmployeeShiftAssignment, Guid>
        _assignmentRepository;

    private readonly IReadRepository<AttendanceShift, Guid>
        _shiftRepository;

    private readonly IReadRepository<AttendancePolicy, Guid>
        _policyRepository;

    public UpdateAttendanceRecordCommandHandler(
        IReadRepository<AttendanceRecord, Guid> recordRepository,
        IReadRepository<EmployeeShiftAssignment, Guid> assignmentRepository,
        IReadRepository<AttendanceShift, Guid> shiftRepository,
        IReadRepository<AttendancePolicy, Guid> policyRepository)
    {
        _recordRepository = recordRepository;
        _assignmentRepository = assignmentRepository;
        _shiftRepository = shiftRepository;
        _policyRepository = policyRepository;
    }

    public async Task Handle(
        UpdateAttendanceRecordCommand request,
        CancellationToken cancellationToken)
    {
        var record = await _recordRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (record is null || record.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance record",
                request.Id);
        }

        var assignment =
            (await _assignmentRepository.FindAsync(
                x =>
                    x.EmployeeId == record.EmployeeId &&
                    x.IsActive &&
                    !x.IsDeleted &&
                    x.EffectiveFrom <= record.AttendanceDate &&
                    (!x.EffectiveTo.HasValue ||
                     x.EffectiveTo.Value >= record.AttendanceDate),
                cancellationToken))
            .OrderByDescending(x => x.IsPrimary)
            .ThenByDescending(x => x.EffectiveFrom)
            .FirstOrDefault();

        if (assignment is null)
        {
            throw new NotFoundException(
                "Employee shift assignment",
                record.EmployeeId);
        }

        var shift = await _shiftRepository.GetByIdAsync(
            assignment.AttendanceShiftId,
            cancellationToken);

        if (shift is null ||
            shift.IsDeleted ||
            !shift.IsActive)
        {
            throw new NotFoundException(
                "Attendance shift",
                assignment.AttendanceShiftId);
        }

        var policy = await _policyRepository.GetByIdAsync(
            assignment.AttendancePolicyId,
            cancellationToken);

        if (policy is null ||
            policy.IsDeleted ||
            !policy.IsActive)
        {
            throw new NotFoundException(
                "Attendance policy",
                assignment.AttendancePolicyId);
        }

        record.CheckIn = request.CheckIn;
        record.CheckOut = request.CheckOut;
        record.Remarks = request.Remarks;

        Recalculate(
            record,
            shift,
            policy);

        await Task.CompletedTask;
    }

    private static void Recalculate(
        AttendanceRecord record,
        AttendanceShift shift,
        AttendancePolicy policy)
    {
        record.WorkedMinutes = 0;
        record.LateMinutes = 0;
        record.EarlyLeaveMinutes = 0;
        record.OvertimeMinutes = 0;

        if (!record.CheckIn.HasValue)
        {
            record.Status = "MissingIn";
            return;
        }

        if (!record.CheckOut.HasValue)
        {
            record.Status = "MissingOut";
            return;
        }

        if (record.CheckOut.Value <= record.CheckIn.Value)
        {
            record.Status = "MissingOut";
            return;
        }

        var elapsedMinutes =
            (int)(
                record.CheckOut.Value -
                record.CheckIn.Value)
            .TotalMinutes;

        record.WorkedMinutes =
            Math.Max(
                0,
                elapsedMinutes -
                shift.BreakMinutes);

        var scheduledStart =
            record.AttendanceDate.ToDateTime(
                shift.StartTime);

        var scheduledEnd =
            record.AttendanceDate.ToDateTime(
                shift.EndTime);

        var checkInLocal =
            record.CheckIn.Value.LocalDateTime;

        var checkOutLocal =
            record.CheckOut.Value.LocalDateTime;

        var lateThreshold =
            scheduledStart.AddMinutes(
                policy.GracePeriodMinutes);

        if (checkInLocal > lateThreshold)
        {
            record.LateMinutes =
                (int)(
                    checkInLocal -
                    scheduledStart)
                .TotalMinutes;
        }

        if (!shift.IsOvernight &&
            checkOutLocal < scheduledEnd)
        {
            record.EarlyLeaveMinutes =
                (int)(
                    scheduledEnd -
                    checkOutLocal)
                .TotalMinutes;
        }

        if (policy.IsOvertimeAllowed &&
            record.WorkedMinutes >
            policy.FullDayMinutes)
        {
            var overtime =
                record.WorkedMinutes -
                policy.FullDayMinutes;

            if (overtime >=
                policy.MinimumOvertimeMinutes)
            {
                record.OvertimeMinutes =
                    Math.Min(
                        overtime,
                        policy.MaximumOvertimeMinutes);
            }
        }

        if (record.WorkedMinutes <
            policy.HalfDayMinutes)
        {
            record.Status = "HalfDay";
        }
        else if (record.LateMinutes > 0 &&
                 record.OvertimeMinutes > 0)
        {
            record.Status = "LateOvertime";
        }
        else if (record.LateMinutes > 0)
        {
            record.Status = "Late";
        }
        else if (record.EarlyLeaveMinutes > 0)
        {
            record.Status = "EarlyLeave";
        }
        else if (record.OvertimeMinutes > 0)
        {
            record.Status = "Overtime";
        }
        else
        {
            record.Status = "Present";
        }
    }
}

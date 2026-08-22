using HRMS.BuildingBlocks.Application.Abstractions;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.ApproveAttendanceRegularization;

public sealed class ApproveAttendanceRegularizationCommandHandler
    : IRequestHandler<ApproveAttendanceRegularizationCommand>
{
    private static readonly Guid PendingStatusId =
        Guid.Parse("10000000-0000-0000-0000-000000000101");

    private static readonly Guid ApprovedStatusId =
        Guid.Parse("10000000-0000-0000-0000-000000000102");

    private readonly IReadRepository<AttendanceRegularization, Guid>
        _regularizationRepository;

    private readonly IReadRepository<AttendanceRecord, Guid>
        _recordRepository;

    private readonly IReadRepository<EmployeeShiftAssignment, Guid>
        _assignmentRepository;

    private readonly IReadRepository<AttendanceShift, Guid>
        _shiftRepository;

    private readonly IReadRepository<AttendancePolicy, Guid>
        _policyRepository;

    private readonly IWriteRepository<AttendanceRegularization, Guid>
        _regularizationWriteRepository;

    private readonly IWriteRepository<AttendanceRecord, Guid>
        _recordWriteRepository;

    private readonly IUserContext _userContext;

    public ApproveAttendanceRegularizationCommandHandler(
        IReadRepository<AttendanceRegularization, Guid> regularizationRepository,
        IReadRepository<AttendanceRecord, Guid> recordRepository,
        IReadRepository<EmployeeShiftAssignment, Guid> assignmentRepository,
        IReadRepository<AttendanceShift, Guid> shiftRepository,
        IReadRepository<AttendancePolicy, Guid> policyRepository,
        IWriteRepository<AttendanceRegularization, Guid> regularizationWriteRepository,
        IWriteRepository<AttendanceRecord, Guid> recordWriteRepository,
        IUserContext userContext)
    {
        _regularizationRepository = regularizationRepository;
        _recordRepository = recordRepository;
        _assignmentRepository = assignmentRepository;
        _shiftRepository = shiftRepository;
        _policyRepository = policyRepository;
        _regularizationWriteRepository =
            regularizationWriteRepository;
        _recordWriteRepository = recordWriteRepository;
        _userContext = userContext;
    }

    public async Task Handle(
        ApproveAttendanceRegularizationCommand request,
        CancellationToken cancellationToken)
    {
        var regularization =
            await _regularizationRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

        if (regularization is null ||
            regularization.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance regularization",
                request.Id);
        }

        if (regularization.AttendanceRegularizationStatusId !=
            PendingStatusId)
        {
            throw new ConflictException(
                "Only pending attendance regularizations can be approved.");
        }

        var record =
            await _recordRepository.GetByIdAsync(
                regularization.AttendanceRecordId,
                cancellationToken);

        if (record is null || record.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance record",
                regularization.AttendanceRecordId);
        }

        var assignments =
            await _assignmentRepository.FindAsync(
                x =>
                    x.EmployeeId == regularization.EmployeeId &&
                    x.IsActive &&
                    !x.IsDeleted &&
                    x.EffectiveFrom <=
                        regularization.AttendanceDate &&
                    (!x.EffectiveTo.HasValue ||
                     x.EffectiveTo.Value >=
                        regularization.AttendanceDate),
                cancellationToken);

        var assignment = assignments
            .OrderByDescending(x => x.IsPrimary)
            .ThenByDescending(x => x.EffectiveFrom)
            .FirstOrDefault();

        if (assignment is null)
        {
            throw new NotFoundException(
                "Employee shift assignment",
                regularization.EmployeeId);
        }

        var shift =
            await _shiftRepository.GetByIdAsync(
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

        var policy =
            await _policyRepository.GetByIdAsync(
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

        // Apply requested attendance
        record.CheckIn =
            regularization.RequestedCheckIn;

        record.CheckOut =
            regularization.RequestedCheckOut;

        // Recalculate attendance
        Recalculate(
            record,
            shift,
            policy);

        // Approve regularization
        regularization.AttendanceRegularizationStatusId =
            ApprovedStatusId;

        regularization.ApprovedBy =
            _userContext.UserId;

        regularization.ApprovedOn =
            DateTimeOffset.UtcNow;

        regularization.ApprovalRemarks =
            request.Remarks;

        await _recordWriteRepository.UpdateAsync(
            record,
            cancellationToken);

        await _regularizationWriteRepository.UpdateAsync(
            regularization,
            cancellationToken);
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

        if (record.CheckOut.Value <=
            record.CheckIn.Value)
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

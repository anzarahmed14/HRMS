using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendanceRecords.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Commands.ProcessAttendanceRecords;

public sealed class ProcessAttendanceRecordsCommandHandler
    : IRequestHandler<ProcessAttendanceRecordsCommand, int>
{
    private readonly IReadRepository<AttendanceRawLog, Guid> _rawLogRepository;
    private readonly IReadRepository<EmployeeShiftAssignment, Guid> _assignmentRepository;
    private readonly IReadRepository<AttendanceShift, Guid> _shiftRepository;
    private readonly IReadRepository<AttendancePolicy, Guid> _policyRepository;
    private readonly IReadRepository<AttendanceRecord, Guid> _recordRepository;
    private readonly IWriteRepository<AttendanceRecord, Guid> _recordWriteRepository;
    private readonly AttendanceCalendarBusinessRules _calendarRules;

    public ProcessAttendanceRecordsCommandHandler(
        IReadRepository<AttendanceRawLog, Guid> rawLogRepository,
        IReadRepository<EmployeeShiftAssignment, Guid> assignmentRepository,
        IReadRepository<AttendanceShift, Guid> shiftRepository,
        IReadRepository<AttendancePolicy, Guid> policyRepository,
        IReadRepository<AttendanceRecord, Guid> recordRepository,
        IWriteRepository<AttendanceRecord, Guid> recordWriteRepository,
        AttendanceCalendarBusinessRules calendarRules)
    {
        _rawLogRepository = rawLogRepository;
        _assignmentRepository = assignmentRepository;
        _shiftRepository = shiftRepository;
        _policyRepository = policyRepository;
        _recordRepository = recordRepository;
        _recordWriteRepository = recordWriteRepository;
        _calendarRules = calendarRules;
    }

    public async Task<int> Handle(
        ProcessAttendanceRecordsCommand request,
        CancellationToken cancellationToken)
    {
        var rawLogs = await _rawLogRepository.FindAsync(
            x =>
                x.EmployeeId == request.EmployeeId &&
                !x.IsDeleted &&
                x.PunchDateTime.Date >=
                    request.FromDate.ToDateTime(TimeOnly.MinValue) &&
                x.PunchDateTime.Date <=
                    request.ToDate.ToDateTime(TimeOnly.MaxValue),
            cancellationToken);

        var assignments = await _assignmentRepository.FindAsync(
            x =>
                x.EmployeeId == request.EmployeeId &&
                x.IsActive &&
                !x.IsDeleted &&
                x.EffectiveFrom <= request.ToDate &&
                (!x.EffectiveTo.HasValue ||
                 x.EffectiveTo.Value >= request.FromDate),
            cancellationToken);

        var shifts = await _shiftRepository.GetAllAsync(
            cancellationToken);

        var policies = await _policyRepository.GetAllAsync(
            cancellationToken);

        var createdCount = 0;

        for (var date = request.FromDate;
             date <= request.ToDate;
             date = date.AddDays(1))
        {
            var assignment = assignments
                .Where(x =>
                    x.EffectiveFrom <= date &&
                    (!x.EffectiveTo.HasValue ||
                     x.EffectiveTo.Value >= date))
                .OrderByDescending(x => x.IsPrimary)
                .ThenByDescending(x => x.EffectiveFrom)
                .FirstOrDefault();

            if (assignment is null)
            {
                continue;
            }

            var shift = shifts.FirstOrDefault(
                x =>
                    x.Id == assignment.AttendanceShiftId &&
                    x.IsActive &&
                    !x.IsDeleted);

            var policy = policies.FirstOrDefault(
                x =>
                    x.Id == assignment.AttendancePolicyId &&
                    x.IsActive &&
                    !x.IsDeleted);

            if (shift is null || policy is null)
            {
                continue;
            }

            var existing = await _recordRepository.FirstOrDefaultAsync(
                x =>
                    x.EmployeeId == request.EmployeeId &&
                    x.AttendanceDate == date &&
                    !x.IsDeleted,
                cancellationToken);

            if (existing is not null)
            {
                continue;
            }

            /*
             * ---------------------------------------------------------
             * 1. WEEKLY OFF
             * ---------------------------------------------------------
             */

            if (_calendarRules.IsWeeklyOff(date))
            {
                await CreateCalendarRecordAsync(
                    request.EmployeeId,
                    assignment,
                    date,
                    "WeeklyOff",
                    "Weekly off.",
                    cancellationToken);

                createdCount++;
                continue;
            }

            /*
             * ---------------------------------------------------------
             * 2. COMPANY HOLIDAY
             * ---------------------------------------------------------
             */

            var isHoliday =
                await _calendarRules.IsHolidayAsync(
                    date,
                    cancellationToken);

            if (isHoliday)
            {
                await CreateCalendarRecordAsync(
                    request.EmployeeId,
                    assignment,
                    date,
                    "Holiday",
                    "Company holiday.",
                    cancellationToken);

                createdCount++;
                continue;
            }

            /*
             * ---------------------------------------------------------
             * 3. APPROVED LEAVE
             * ---------------------------------------------------------
             */

            var isLeave =
                await _calendarRules.IsApprovedLeaveAsync(
                    request.EmployeeId,
                    date,
                    cancellationToken);

            if (isLeave)
            {
                await CreateCalendarRecordAsync(
                    request.EmployeeId,
                    assignment,
                    date,
                    "Leave",
                    "Approved leave.",
                    cancellationToken);

                createdCount++;
                continue;
            }

            /*
             * ---------------------------------------------------------
             * 4. RAW ATTENDANCE LOGS
             * ---------------------------------------------------------
             */

            var dailyLogs = rawLogs
                .Where(x =>
                    DateOnly.FromDateTime(
                        x.PunchDateTime.LocalDateTime) == date)
                .OrderBy(x => x.PunchDateTime)
                .ToList();

            /*
             * ---------------------------------------------------------
             * 5. NO LOG = ABSENT
             * ---------------------------------------------------------
             */

            if (dailyLogs.Count == 0)
            {
                await CreateCalendarRecordAsync(
                    request.EmployeeId,
                    assignment,
                    date,
                    "Absent",
                    "No attendance punches found.",
                    cancellationToken);

                createdCount++;
                continue;
            }

            /*
             * ---------------------------------------------------------
             * 6. CALCULATE ATTENDANCE
             * ---------------------------------------------------------
             */

            var record = BuildAttendanceRecord(
                request.EmployeeId,
                assignment,
                shift,
                policy,
                date,
                dailyLogs);

            await _recordWriteRepository.AddAsync(
                record,
                cancellationToken);

            createdCount++;
        }

        return createdCount;
    }

    private async Task CreateCalendarRecordAsync(
        Guid employeeId,
        EmployeeShiftAssignment assignment,
        DateOnly date,
        string status,
        string remarks,
        CancellationToken cancellationToken)
    {
        var record = new AttendanceRecord
        {
            Id = Guid.NewGuid(),

            EmployeeId = employeeId,

            AttendanceShiftId =
                assignment.AttendanceShiftId,

            AttendancePolicyId =
                assignment.AttendancePolicyId,

            AttendanceDate = date,

            WorkedMinutes = 0,

            LateMinutes = 0,

            EarlyLeaveMinutes = 0,

            OvertimeMinutes = 0,

            Status = status,

            Remarks = remarks
        };

        await _recordWriteRepository.AddAsync(
            record,
            cancellationToken);
    }

    private static AttendanceRecord BuildAttendanceRecord(
        Guid employeeId,
        EmployeeShiftAssignment assignment,
        AttendanceShift shift,
        AttendancePolicy policy,
        DateOnly date,
        List<AttendanceRawLog> logs)
    {
        var record = new AttendanceRecord
        {
            Id = Guid.NewGuid(),

            EmployeeId = employeeId,

            AttendanceShiftId =
                assignment.AttendanceShiftId,

            AttendancePolicyId =
                assignment.AttendancePolicyId,

            AttendanceDate = date
        };

        var inPunches = logs
            .Where(x =>
                string.Equals(
                    x.PunchType,
                    "IN",
                    StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x.PunchDateTime)
            .ToList();

        var outPunches = logs
            .Where(x =>
                string.Equals(
                    x.PunchType,
                    "OUT",
                    StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.PunchDateTime)
            .ToList();

        var firstIn = inPunches.FirstOrDefault();

        var lastOut = outPunches.FirstOrDefault();

        /*
         * ---------------------------------------------------------
         * MISSING IN
         * ---------------------------------------------------------
         */

        if (firstIn is null)
        {
            record.CheckOut = lastOut?.PunchDateTime;

            record.Status = "MissingIn";

            record.Remarks =
                "IN punch is missing.";

            return record;
        }

        record.CheckIn = firstIn.PunchDateTime;

        /*
         * ---------------------------------------------------------
         * MISSING OUT
         * ---------------------------------------------------------
         */

        if (lastOut is null ||
            lastOut.PunchDateTime <=
            firstIn.PunchDateTime)
        {
            record.Status = "MissingOut";

            record.Remarks =
                "OUT punch is missing.";

            return record;
        }

        record.CheckOut = lastOut.PunchDateTime;

        /*
         * ---------------------------------------------------------
         * WORKED MINUTES
         * ---------------------------------------------------------
         */

        var elapsedMinutes =
            (int)(
                lastOut.PunchDateTime -
                firstIn.PunchDateTime)
            .TotalMinutes;

        record.WorkedMinutes =
            Math.Max(
                0,
                elapsedMinutes -
                shift.BreakMinutes);

        /*
         * ---------------------------------------------------------
         * SHIFT TIME
         * ---------------------------------------------------------
         */

        var scheduledStart =
            date.ToDateTime(
                shift.StartTime);

        var scheduledEnd =
            date.ToDateTime(
                shift.EndTime);

        /*
         * ---------------------------------------------------------
         * LATE
         * ---------------------------------------------------------
         */

        var checkInLocal =
            firstIn.PunchDateTime.LocalDateTime;

        var lateThreshold =
            scheduledStart.AddMinutes(
                policy.GracePeriodMinutes);

        if (checkInLocal > lateThreshold)
        {
            record.LateMinutes =
                Math.Max(
                    0,
                    (int)(
                        checkInLocal -
                        scheduledStart)
                    .TotalMinutes);
        }

        /*
         * ---------------------------------------------------------
         * EARLY LEAVE
         * ---------------------------------------------------------
         */

        var checkOutLocal =
            lastOut.PunchDateTime.LocalDateTime;

        if (!shift.IsOvernight &&
            checkOutLocal < scheduledEnd)
        {
            record.EarlyLeaveMinutes =
                Math.Max(
                    0,
                    (int)(
                        scheduledEnd -
                        checkOutLocal)
                    .TotalMinutes);
        }

        /*
         * ---------------------------------------------------------
         * OVERTIME
         * ---------------------------------------------------------
         */

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

        /*
         * ---------------------------------------------------------
         * STATUS
         * ---------------------------------------------------------
         */

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

        return record;
    }
}
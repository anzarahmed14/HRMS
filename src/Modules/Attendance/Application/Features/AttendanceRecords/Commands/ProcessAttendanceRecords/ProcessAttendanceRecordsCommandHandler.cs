using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Services;
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

    private readonly IAttendanceCalculationService _calculationService;
    private readonly IAttendanceDayStatusService _dayStatusService;

    public ProcessAttendanceRecordsCommandHandler(
        IReadRepository<AttendanceRawLog, Guid> rawLogRepository,
        IReadRepository<EmployeeShiftAssignment, Guid> assignmentRepository,
        IReadRepository<AttendanceShift, Guid> shiftRepository,
        IReadRepository<AttendancePolicy, Guid> policyRepository,
        IReadRepository<AttendanceRecord, Guid> recordRepository,
        IWriteRepository<AttendanceRecord, Guid> recordWriteRepository,
        IAttendanceCalculationService calculationService,
        IAttendanceDayStatusService dayStatusService)
    {
        _rawLogRepository = rawLogRepository;
        _assignmentRepository = assignmentRepository;
        _shiftRepository = shiftRepository;
        _policyRepository = policyRepository;
        _recordRepository = recordRepository;
        _recordWriteRepository = recordWriteRepository;
        _calculationService = calculationService;
        _dayStatusService = dayStatusService;
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

            var existing =
                await _recordRepository.FirstOrDefaultAsync(
                    x =>
                        x.EmployeeId == request.EmployeeId &&
                        x.AttendanceDate == date &&
                        !x.IsDeleted,
                    cancellationToken);

            if (existing is not null)
            {
                continue;
            }

            // ---------------------------------------------------------
            // 1. DETERMINE DAY STATUS
            // ---------------------------------------------------------

            var dayStatus =
                await _dayStatusService.DetermineAsync(
                    request.EmployeeId,
                    date,
                    cancellationToken);

            // ---------------------------------------------------------
            // 1A. HALF-DAY APPROVED LEAVE
            // ---------------------------------------------------------

            if (dayStatus.Code ==
                    AttendanceDayStatusCodes.Leave &&
                dayStatus.LeaveDays == 0.50m)
            {
                await CreateCalendarRecordAsync(
                    request.EmployeeId,
                    assignment,
                    date,
                    "HalfDay",
                    dayStatus.Remarks ?? "Approved half-day leave.",
                    cancellationToken);

                createdCount++;

                continue;
            }

            // ---------------------------------------------------------
            // 1B. OTHER NON-WORKING DAYS
            // ---------------------------------------------------------

            if (dayStatus.Code !=
                AttendanceDayStatusCodes.WorkingDay)
            {
                await CreateCalendarRecordAsync(
                    request.EmployeeId,
                    assignment,
                    date,
                    dayStatus.Code,
                    dayStatus.Remarks ?? string.Empty,
                    cancellationToken);

                createdCount++;

                continue;
            }

            // ---------------------------------------------------------
            // 2. RAW ATTENDANCE LOGS
            // ---------------------------------------------------------

            var dailyLogs = rawLogs
                .Where(x =>
                    DateOnly.FromDateTime(
                        x.PunchDateTime.LocalDateTime) == date)
                .OrderBy(x => x.PunchDateTime)
                .ToList();

            // ---------------------------------------------------------
            // 3. NO LOG = ABSENT
            // ---------------------------------------------------------

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

            // ---------------------------------------------------------
            // 4. BUILD BASIC ATTENDANCE RECORD
            // ---------------------------------------------------------

            var record = BuildAttendanceRecord(
                request.EmployeeId,
                assignment,
                date,
                dailyLogs);

            // ---------------------------------------------------------
            // 5. CALCULATE ATTENDANCE
            // ---------------------------------------------------------

            _calculationService.Calculate(
                record,
                shift,
                policy);

            // ---------------------------------------------------------
            // 6. SAVE ATTENDANCE RECORD
            // ---------------------------------------------------------

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

        var firstIn =
            logs
                .Where(x =>
                    string.Equals(
                        x.PunchType,
                        "IN",
                        StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.PunchDateTime)
                .FirstOrDefault();

        var lastOut =
            logs
                .Where(x =>
                    string.Equals(
                        x.PunchType,
                        "OUT",
                        StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x => x.PunchDateTime)
                .FirstOrDefault();

        record.CheckIn =
            firstIn?.PunchDateTime;

        record.CheckOut =
            lastOut?.PunchDateTime;

        return record;
    }
}
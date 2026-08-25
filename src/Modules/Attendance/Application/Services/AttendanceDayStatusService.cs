using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendanceRecords.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;

namespace HRMS.Modules.Attendance.Application.Services;

public sealed class AttendanceDayStatusService
    : IAttendanceDayStatusService
{
    private readonly AttendanceCalendarBusinessRules _calendarRules;

    private readonly IReadRepository<AttendanceDayStatus, Guid>
        _dayStatusRepository;

    public AttendanceDayStatusService(
        AttendanceCalendarBusinessRules calendarRules,
        IReadRepository<AttendanceDayStatus, Guid> dayStatusRepository)
    {
        _calendarRules = calendarRules;
        _dayStatusRepository = dayStatusRepository;
    }

    public async Task<AttendanceDayStatusResult> DetermineAsync(
        Guid employeeId,
        DateOnly date,
        CancellationToken cancellationToken = default)
    {
        // ---------------------------------------------------------
        // 1. WEEKLY OFF
        // ---------------------------------------------------------

        if (_calendarRules.IsWeeklyOff(date))
        {
            return await CreateResultAsync(
                AttendanceDayStatusCodes.WeeklyOff,
                "Weekly off.",
                cancellationToken);
        }

        // ---------------------------------------------------------
        // 2. COMPANY HOLIDAY
        // ---------------------------------------------------------

        var isHoliday =
            await _calendarRules.IsHolidayAsync(
                date,
                cancellationToken);

        if (isHoliday)
        {
            return await CreateResultAsync(
                AttendanceDayStatusCodes.Holiday,
                "Company holiday.",
                cancellationToken);
        }

        // ---------------------------------------------------------
        // 3. APPROVED LEAVE
        // ---------------------------------------------------------

        var leave =
            await _calendarRules.IsApprovedLeaveAsync(
                employeeId,
                date,
                cancellationToken);

        if (leave is not null)
        {
            var remarks =
                leave.TotalDays == 0.50m
                    ? "Approved half-day leave."
                    : "Approved leave.";

            return await CreateResultAsync(
                AttendanceDayStatusCodes.Leave,
                remarks,
                cancellationToken,
                leave.TotalDays);
        }

        // ---------------------------------------------------------
        // 4. WORKING DAY
        // ---------------------------------------------------------

        return await CreateResultAsync(
            AttendanceDayStatusCodes.WorkingDay,
            null,
            cancellationToken);
    }

    private async Task<AttendanceDayStatusResult> CreateResultAsync(
        string code,
        string? remarks,
        CancellationToken cancellationToken,
        decimal? leaveDays = null)
    {
        var status =
            await _dayStatusRepository.FirstOrDefaultAsync(
                x =>
                    x.Code == code &&
                    x.IsActive &&
                    !x.IsDeleted,
                cancellationToken);

        if (status is null)
        {
            throw new InvalidOperationException(
                $"Attendance day status '{code}' was not found.");
        }

        return new AttendanceDayStatusResult(
            status.Id,
            status.Code,
            remarks,
            leaveDays);
    }
}
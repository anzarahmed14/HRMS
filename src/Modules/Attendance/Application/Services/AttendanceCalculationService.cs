using HRMS.Modules.Attendance.Domain.Entities;

namespace HRMS.Modules.Attendance.Application.Services;

public sealed class AttendanceCalculationService : IAttendanceCalculationService
{
    public void Calculate(
        AttendanceRecord record,
        AttendanceShift shift,
        AttendancePolicy policy)
    {
        ResetCalculation(record);

        // ---------------------------------------------------------
        // 1. CHECK-IN / CHECK-OUT VALIDATION
        // ---------------------------------------------------------

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

        // ---------------------------------------------------------
        // 2. WORKED MINUTES
        // ---------------------------------------------------------

        var elapsedMinutes =
            (int)(
                record.CheckOut.Value -
                record.CheckIn.Value)
            .TotalMinutes;

        record.WorkedMinutes =
            Math.Max(
                0,
                elapsedMinutes - shift.BreakMinutes);

        // ---------------------------------------------------------
        // 3. SCHEDULED SHIFT TIME
        // ---------------------------------------------------------

        var scheduledStart =
            record.AttendanceDate.ToDateTime(
                shift.StartTime);

        var scheduledEnd =
            record.AttendanceDate.ToDateTime(
                shift.EndTime);

        // Overnight shift:
        //
        // Example:
        // 22:00 → 06:00
        //
        // End belongs to the next day.
        if (shift.IsOvernight &&
            shift.EndTime <= shift.StartTime)
        {
            scheduledEnd =
                scheduledEnd.AddDays(1);
        }

        var checkInLocal =
            record.CheckIn.Value.LocalDateTime;

        var checkOutLocal =
            record.CheckOut.Value.LocalDateTime;

        // ---------------------------------------------------------
        // 4. LATE ARRIVAL
        // ---------------------------------------------------------

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

        // ---------------------------------------------------------
        // 5. EARLY LEAVE
        // ---------------------------------------------------------

        if (checkOutLocal < scheduledEnd)
        {
            record.EarlyLeaveMinutes =
                Math.Max(
                    0,
                    (int)(
                        scheduledEnd -
                        checkOutLocal)
                    .TotalMinutes);
        }

        // ---------------------------------------------------------
        // 6. OVERTIME
        // ---------------------------------------------------------

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

        // ---------------------------------------------------------
        // 7. FINAL ATTENDANCE STATUS
        // ---------------------------------------------------------

        record.Status =
            DetermineStatus(record);
    }

    private static void ResetCalculation(
        AttendanceRecord record)
    {
        record.WorkedMinutes = 0;
        record.LateMinutes = 0;
        record.EarlyLeaveMinutes = 0;
        record.OvertimeMinutes = 0;
    }

    private static string DetermineStatus(
        AttendanceRecord record)
    {
        if (record.WorkedMinutes == 0)
        {
            return "Absent";
        }

        if (record.WorkedMinutes < 240)
        {
            return "HalfDay";
        }

        if (record.LateMinutes > 0 &&
            record.OvertimeMinutes > 0)
        {
            return "LateOvertime";
        }

        if (record.LateMinutes > 0)
        {
            return "Late";
        }

        if (record.EarlyLeaveMinutes > 0)
        {
            return "EarlyLeave";
        }

        if (record.OvertimeMinutes > 0)
        {
            return "Overtime";
        }

        return "Present";
    }
}
using HRMS.Modules.Attendance.Domain.Entities;

namespace HRMS.Modules.Attendance.Application.Services;

public interface IAttendanceCalculationService
{
    void Calculate(
        AttendanceRecord record,
        AttendanceShift shift,
        AttendancePolicy policy);
}

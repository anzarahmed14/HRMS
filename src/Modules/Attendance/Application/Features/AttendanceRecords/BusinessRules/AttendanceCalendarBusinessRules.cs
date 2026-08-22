using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.BusinessRules;

public sealed class AttendanceCalendarBusinessRules
{
    private readonly IReadRepository<CompanyHoliday, Guid>
        _holidayRepository;

    private readonly IReadRepository<LeaveRequest, Guid>
        _leaveRequestRepository;

    private readonly IReadRepository<LeaveRequestStatus, Guid>
        _leaveStatusRepository;

    public AttendanceCalendarBusinessRules(
        IReadRepository<CompanyHoliday, Guid> holidayRepository,
        IReadRepository<LeaveRequest, Guid> leaveRequestRepository,
        IReadRepository<LeaveRequestStatus, Guid> leaveStatusRepository)
    {
        _holidayRepository = holidayRepository;
        _leaveRequestRepository = leaveRequestRepository;
        _leaveStatusRepository = leaveStatusRepository;
    }

    public bool IsWeeklyOff(DateOnly date)
    {
        return date.DayOfWeek is
            DayOfWeek.Saturday or
            DayOfWeek.Sunday;
    }

    public async Task<bool> IsHolidayAsync(
        DateOnly date,
        CancellationToken cancellationToken = default)
    {
        return await _holidayRepository.AnyAsync(
            x =>
                x.HolidayDate == date &&
                x.IsActive &&
                !x.IsDeleted,
            cancellationToken);
    }

    public async Task<bool> IsApprovedLeaveAsync(
        Guid employeeId,
        DateOnly date,
        CancellationToken cancellationToken = default)
    {
        var approvedStatus = await _leaveStatusRepository
            .FirstOrDefaultAsync(
                x =>
                    x.Code == "APPROVED" &&
                    x.IsActive &&
                    !x.IsDeleted,
                cancellationToken);

        if (approvedStatus is null)
        {
            return false;
        }

        return await _leaveRequestRepository.AnyAsync(
            x =>
                x.EmployeeId == employeeId &&
                x.StatusId == approvedStatus.Id &&
                x.FromDate <= date &&
                x.ToDate >= date &&
                !x.IsDeleted,
            cancellationToken);
    }
}

using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;
using EmployeeEntity = HRMS.Modules.Employee.Domain.Entities.Employee;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.BusinessRules;

public class LeaveRequestBusinessRules
{
    private const string ActiveStatusCode = "ACTIVE";

    private const string DraftStatusCode = "DRAFT";
    private const string PendingStatusCode = "PENDING";
    private const string ApprovedStatusCode = "APPROVED";
    private const string RejectedStatusCode = "REJECTED";
    private const string CancelledStatusCode = "CANCELLED";

    private readonly IReadRepository<EmployeeEntity, Guid> _employeeRepository;
    private readonly IReadRepository<LeaveYear, Guid> _leaveYearRepository;
    private readonly IReadRepository<LeaveType, Guid> _leaveTypeRepository;
    private readonly IReadRepository<LeaveDayPart, Guid> _leaveDayPartRepository;
    private readonly IReadRepository<LeaveRequestStatus, Guid> _statusRepository;
    private readonly IReadRepository<LeaveRequest, Guid> _leaveRequestRepository;
    private readonly IReadRepository<CompanyHoliday, Guid> _companyHolidayRepository;
    private readonly IReadRepository<LeaveYearStatus, Guid> _leaveYearStatusRepository;

    public LeaveRequestBusinessRules(
        IReadRepository<EmployeeEntity, Guid> employeeRepository,
        IReadRepository<LeaveYear, Guid> leaveYearRepository,
        IReadRepository<LeaveType, Guid> leaveTypeRepository,
        IReadRepository<LeaveDayPart, Guid> leaveDayPartRepository,
        IReadRepository<LeaveRequestStatus, Guid> statusRepository,
        IReadRepository<LeaveYearStatus, Guid> leaveYearStatusRepository,
        IReadRepository<LeaveRequest, Guid> leaveRequestRepository,
        IReadRepository<CompanyHoliday, Guid> companyHolidayRepository)
    {
        _employeeRepository = employeeRepository;
        _leaveYearRepository = leaveYearRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _leaveDayPartRepository = leaveDayPartRepository;
        _statusRepository = statusRepository;
        _leaveYearStatusRepository = leaveYearStatusRepository;
        _leaveRequestRepository = leaveRequestRepository;
        _companyHolidayRepository = companyHolidayRepository;
    }

    public async Task<EmployeeEntity> EnsureEmployeeIsValidAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(
            employeeId,
            cancellationToken);

        if (employee is null || employee.IsDeleted)
        {
            throw new NotFoundException(
                "Employee",
                employeeId);
        }

        return employee;
    }

    public async Task<LeaveYear> EnsureLeaveYearIsValidAsync(
        Guid leaveYearId,
        CancellationToken cancellationToken = default)
    {
        var leaveYear = await _leaveYearRepository.GetByIdAsync(
            leaveYearId,
            cancellationToken);

        if (leaveYear is null || leaveYear.IsDeleted)
        {
            throw new NotFoundException(
                "Leave Year",
                leaveYearId);
        }

        var activeStatus = await GetLeaveYearActiveStatusAsync(
            cancellationToken);

        if (leaveYear.StatusId != activeStatus.Id)
        {
            throw new ConflictException(
                "Leave year must be active to apply leave.");
        }

        return leaveYear;
    }

    public async Task<LeaveType> EnsureLeaveTypeIsValidAsync(
        Guid leaveTypeId,
        CancellationToken cancellationToken = default)
    {
        var leaveType = await _leaveTypeRepository.GetByIdAsync(
            leaveTypeId,
            cancellationToken);

        if (leaveType is null || leaveType.IsDeleted)
        {
            throw new NotFoundException(
                "Leave Type",
                leaveTypeId);
        }

        if (!leaveType.IsActive)
        {
            throw new ConflictException(
                "Leave type is not active.");
        }

        return leaveType;
    }

    public async Task<LeaveDayPart> EnsureLeaveDayPartIsValidAsync(
        Guid leaveDayPartId,
        CancellationToken cancellationToken = default)
    {
        var dayPart = await _leaveDayPartRepository.GetByIdAsync(
            leaveDayPartId,
            cancellationToken);

        if (dayPart is null)
        {
            throw new NotFoundException(
                "Leave Day Part",
                leaveDayPartId);
        }

        if (!dayPart.IsActive)
        {
            throw new ConflictException(
                "Leave day part is not active.");
        }

        return dayPart;
    }

    public void EnsureDatesAreValid(
        DateOnly fromDate,
        DateOnly toDate,
        LeaveYear leaveYear)
    {
        if (fromDate > toDate)
        {
            throw new ConflictException(
                "From date cannot be greater than to date.");
        }

        if (fromDate < leaveYear.StartDate ||
            toDate > leaveYear.EndDate)
        {
            throw new ConflictException(
                "Leave request dates must fall within the selected leave year.");
        }
    }

    public void EnsureDayPartsAreValid(
        DateOnly fromDate,
        DateOnly toDate,
        LeaveDayPart startDayPart,
        LeaveDayPart endDayPart)
    {
        if (fromDate < toDate)
        {
            return;
        }

        if (startDayPart.Code == "SECOND_HALF" &&
            endDayPart.Code == "FIRST_HALF")
        {
            throw new ConflictException(
                "Second half cannot be followed by first half on the same day.");
        }
    }

    public decimal CalculateTotalDays(
        DateOnly fromDate,
        DateOnly toDate,
        LeaveDayPart startDayPart,
        LeaveDayPart endDayPart)
    {
        if (fromDate == toDate)
        {
            if (startDayPart.Code == "FIRST_HALF" &&
                endDayPart.Code == "SECOND_HALF")
            {
                return 1.00m;
            }

            return startDayPart.DaysValue;
        }

        var totalDays = startDayPart.DaysValue;

        var fullDaysBetween =
            toDate.DayNumber - fromDate.DayNumber - 1;

        if (fullDaysBetween > 0)
        {
            totalDays += fullDaysBetween;
        }

        totalDays += endDayPart.DaysValue;

        return totalDays;
    }
    /// <summary>
    /// Calculates leave days while excluding mandatory company holidays
    /// belonging to the selected leave year.
    /// </summary>
    public async Task<decimal> CalculateTotalDaysAsync(
        Guid leaveYearId,
        DateOnly fromDate,
        DateOnly toDate,
        LeaveDayPart startDayPart,
        LeaveDayPart endDayPart,
        CancellationToken cancellationToken = default)
    {
        // Preserve the existing calculation as the baseline.
        var totalDays = CalculateTotalDays(
            fromDate,
            toDate,
            startDayPart,
            endDayPart);

        if (totalDays <= 0)
        {
            return totalDays;
        }

        // Only mandatory, active and non-deleted company holidays
        // belonging to the selected LeaveYear are considered.
        var holidays = await _companyHolidayRepository.FindAsync(
            x =>
                x.LeaveYearId == leaveYearId &&
                x.HolidayDate >= fromDate &&
                x.HolidayDate <= toDate &&
                x.IsActive &&
                !x.IsOptional &&
                !x.IsDeleted,
            cancellationToken);

        if (!holidays.Any())
        {
            return totalDays;
        }

        foreach (var holiday in holidays)
        {
            // A holiday in the middle of the leave period
            // represents one complete excluded leave day.
            if (holiday.HolidayDate > fromDate &&
                holiday.HolidayDate < toDate)
            {
                totalDays -= 1m;
            }
        }

        return Math.Max(0m, totalDays);
    }

    public async Task EnsureNoOverlappingLeaveRequestAsync(
        Guid employeeId,
        DateOnly fromDate,
        DateOnly toDate,
        Guid? excludeLeaveRequestId = null,
        CancellationToken cancellationToken = default)
    {
        var pendingStatus = await GetStatusByCodeAsync(
            PendingStatusCode,
            cancellationToken);

        var approvedStatus = await GetStatusByCodeAsync(
            ApprovedStatusCode,
            cancellationToken);

        var exists = await _leaveRequestRepository.AnyAsync(
            x =>
                x.EmployeeId == employeeId &&
                !x.IsDeleted &&
                (!excludeLeaveRequestId.HasValue ||
                 x.Id != excludeLeaveRequestId.Value) &&
                (
                    x.StatusId == pendingStatus.Id ||
                    x.StatusId == approvedStatus.Id
                ) &&
                x.FromDate <= toDate &&
                x.ToDate >= fromDate,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Employee already has a pending or approved leave request for the selected dates.");
        }
    }

    // ============================================================
    // STATE TRANSITION RULES
    // ============================================================

    public async Task EnsureCanSubmitAsync(
        LeaveRequest leaveRequest,
        CancellationToken cancellationToken = default)
    {
        var draftStatus = await GetStatusByCodeAsync(
            DraftStatusCode,
            cancellationToken);

        if (leaveRequest.StatusId != draftStatus.Id)
        {
            throw new ConflictException(
                "Only draft leave requests can be submitted.");
        }
    }

    public async Task EnsureCanApproveAsync(
        LeaveRequest leaveRequest,
        CancellationToken cancellationToken = default)
    {
        var pendingStatus = await GetStatusByCodeAsync(
            PendingStatusCode,
            cancellationToken);

        if (leaveRequest.StatusId != pendingStatus.Id)
        {
            throw new ConflictException(
                "Only pending leave requests can be approved.");
        }
    }

    public async Task EnsureCanRejectAsync(
        LeaveRequest leaveRequest,
        CancellationToken cancellationToken = default)
    {
        var pendingStatus = await GetStatusByCodeAsync(
            PendingStatusCode,
            cancellationToken);

        if (leaveRequest.StatusId != pendingStatus.Id)
        {
            throw new ConflictException(
                "Only pending leave requests can be rejected.");
        }
    }

    public async Task EnsureCanCancelAsync(
        LeaveRequest leaveRequest,
        CancellationToken cancellationToken = default)
    {
        var approvedStatus = await GetStatusByCodeAsync(
            ApprovedStatusCode,
            cancellationToken);

        if (leaveRequest.StatusId != approvedStatus.Id)
        {
            throw new ConflictException(
                "Only approved leave requests can be cancelled.");
        }
    }

    public async Task EnsureCanUpdateAsync(
        LeaveRequest leaveRequest,
        CancellationToken cancellationToken = default)
    {
        var draftStatus = await GetStatusByCodeAsync(
            DraftStatusCode,
            cancellationToken);

        if (leaveRequest.StatusId != draftStatus.Id)
        {
            throw new ConflictException(
                "Only draft leave requests can be updated.");
        }
    }

    public async Task EnsureCanDeleteAsync(
        LeaveRequest leaveRequest,
        CancellationToken cancellationToken = default)
    {
        var draftStatus = await GetStatusByCodeAsync(
            DraftStatusCode,
            cancellationToken);

        if (leaveRequest.StatusId != draftStatus.Id)
        {
            throw new ConflictException(
                "Only draft leave requests can be deleted.");
        }
    }

    // ============================================================
    // STATUS ID HELPERS
    // ============================================================

    public async Task<Guid> GetDraftStatusIdAsync(
        CancellationToken cancellationToken = default)
    {
        var status = await GetStatusByCodeAsync(
            DraftStatusCode,
            cancellationToken);

        return status.Id;
    }

    public async Task<Guid> GetPendingStatusIdAsync(
        CancellationToken cancellationToken = default)
    {
        var status = await GetStatusByCodeAsync(
            PendingStatusCode,
            cancellationToken);

        return status.Id;
    }

    public async Task<Guid> GetApprovedStatusIdAsync(
        CancellationToken cancellationToken = default)
    {
        var status = await GetStatusByCodeAsync(
            ApprovedStatusCode,
            cancellationToken);

        return status.Id;
    }

    public async Task<Guid> GetRejectedStatusIdAsync(
        CancellationToken cancellationToken = default)
    {
        var status = await GetStatusByCodeAsync(
            RejectedStatusCode,
            cancellationToken);

        return status.Id;
    }

    public async Task<Guid> GetCancelledStatusIdAsync(
        CancellationToken cancellationToken = default)
    {
        var status = await GetStatusByCodeAsync(
            CancelledStatusCode,
            cancellationToken);

        return status.Id;
    }

    private async Task<LeaveYearStatus> GetLeaveYearActiveStatusAsync(
        CancellationToken cancellationToken)
    {
        var status = await _leaveYearStatusRepository.FirstOrDefaultAsync(
            x =>
                x.Code == ActiveStatusCode &&
                x.IsActive &&
                !x.IsDeleted,
            cancellationToken);

        if (status is null)
        {
            throw new NotFoundException(
                "Leave Year Status",
                ActiveStatusCode);
        }

        return status;
    }

    private async Task<LeaveRequestStatus> GetStatusByCodeAsync(
        string code,
        CancellationToken cancellationToken)
    {
        var status = await _statusRepository.FirstOrDefaultAsync(
            x =>
                x.Code == code &&
                x.IsActive &&
                !x.IsDeleted,
            cancellationToken);

        if (status is null)
        {
            throw new NotFoundException(
                "Leave Request Status",
                code);
        }

        return status;
    }
}


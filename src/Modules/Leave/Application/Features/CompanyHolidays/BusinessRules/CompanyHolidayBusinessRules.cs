using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.BusinessRules;

public sealed class CompanyHolidayBusinessRules
{
    private readonly IReadRepository<LeaveYear, Guid> _leaveYearRepository;
    private readonly IReadRepository<CompanyHoliday, Guid> _holidayRepository;

    public CompanyHolidayBusinessRules(
        IReadRepository<LeaveYear, Guid> leaveYearRepository,
        IReadRepository<CompanyHoliday, Guid> holidayRepository)
    {
        _leaveYearRepository = leaveYearRepository;
        _holidayRepository = holidayRepository;
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

        return leaveYear;
    }

    public void EnsureHolidayDateIsWithinLeaveYear(
        DateOnly holidayDate,
        LeaveYear leaveYear)
    {
        if (holidayDate < leaveYear.StartDate ||
            holidayDate > leaveYear.EndDate)
        {
            throw new ConflictException(
                "Holiday date must fall within the selected leave year.");
        }
    }

    public async Task EnsureHolidayDoesNotExistAsync(
        Guid leaveYearId,
        DateOnly holidayDate,
        CancellationToken cancellationToken = default)
    {
        var exists = await _holidayRepository.AnyAsync(
            x =>
                x.LeaveYearId == leaveYearId &&
                x.HolidayDate == holidayDate &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "A holiday already exists for this date in the selected leave year.");
        }
    }
}

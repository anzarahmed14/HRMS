using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;
namespace HRMS.Modules.Leave.Application.Features.LeaveYears.BusinessRules;

public class LeaveYearBusinessRules
{
    private readonly IReadRepository<LeaveYear, Guid> _leaveYearRepository;
    private readonly IReadRepository<LeaveYearStatus, Guid>
    _leaveYearStatusRepository;
    public LeaveYearBusinessRules(
        IReadRepository<LeaveYear, Guid> leaveYearRepository, IReadRepository<LeaveYearStatus, Guid> leaveYearStatusRepository)
    {
        _leaveYearRepository = leaveYearRepository;
        _leaveYearStatusRepository = leaveYearStatusRepository;
    }

    public async Task EnsureLeaveYearExistsAsync(
        Guid leaveYearId,
        CancellationToken cancellationToken = default)
    {
        var leaveYear = await _leaveYearRepository.GetByIdAsync(
            leaveYearId,
            cancellationToken);

        if (leaveYear is null)
        {
            throw new NotFoundException(
                "Leave Year",
                leaveYearId);
        }
    }
    public async Task EnsureOnlyOneActiveLeaveYearAsync(
    Guid companyId,
    Guid activeStatusId,
    Guid leaveYearId,
    CancellationToken cancellationToken = default)
    {
        var activeExists = await _leaveYearRepository.AnyAsync(
            x =>
                x.CompanyId == companyId &&
                x.Id != leaveYearId &&
                x.StatusId == activeStatusId &&
                !x.IsDeleted,
            cancellationToken);

        if (activeExists)
        {
            throw new ConflictException(
                "An active leave year already exists for this company.");
        }
    }
    public async Task<bool> IsActiveStatusAsync(
    Guid statusId,
    CancellationToken cancellationToken = default)
    {
        var status = await _leaveYearStatusRepository.GetByIdAsync(
            statusId,
            cancellationToken);

        return status is not null &&
               status.Code.Equals(
                   "ACTIVE",
                   StringComparison.OrdinalIgnoreCase) &&
               status.IsActive;
    }
    public async Task EnsureNoOverlappingLeaveYearAsync(
        Guid companyId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default)
    {
        var overlaps = await _leaveYearRepository.AnyAsync(
            x =>
                x.CompanyId == companyId &&
                !x.IsDeleted &&
                startDate <= x.EndDate &&
                endDate >= x.StartDate,
            cancellationToken);

        if (overlaps)
        {
            throw new ConflictException(
                "Leave year period overlaps with an existing leave year.");
        }
    }

    public async Task EnsureNoOverlappingLeaveYearAsync(
        Guid companyId,
        DateOnly startDate,
        DateOnly endDate,
        Guid leaveYearId,
        CancellationToken cancellationToken = default)
    {
        var overlaps = await _leaveYearRepository.AnyAsync(
            x =>
                x.CompanyId == companyId &&
                x.Id != leaveYearId &&
                !x.IsDeleted &&
                startDate <= x.EndDate &&
                endDate >= x.StartDate,
            cancellationToken);

        if (overlaps)
        {
            throw new ConflictException(
                "Leave year period overlaps with an existing leave year.");
        }
    }

    public async Task EnsureOnlyOneActiveLeaveYearAsync(
        Guid companyId,
        Guid activeStatusId,
        CancellationToken cancellationToken = default)
    {
        var activeExists = await _leaveYearRepository.AnyAsync(
            x =>
                x.CompanyId == companyId &&
                x.StatusId == activeStatusId &&
                !x.IsDeleted,
            cancellationToken);

        if (activeExists)
        {
            throw new ConflictException(
                "An active leave year already exists for this company.");
        }
    }

    public async Task EnsureLeaveYearDatesAreValidAsync(
        DateOnly startDate,
        DateOnly endDate)
    {
        if (startDate >= endDate)
        {
            throw new ConflictException(
                "Leave year start date must be earlier than end date.");
        }
    }
}
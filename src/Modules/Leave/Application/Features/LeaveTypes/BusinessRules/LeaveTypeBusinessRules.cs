using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Leave.Application.Features.LeaveTypes.BusinessRules;

public class LeaveTypeBusinessRules
{
    private readonly IReadRepository<LeaveType, Guid> _leaveTypeRepository;

    public LeaveTypeBusinessRules(
        IReadRepository<LeaveType, Guid> leaveTypeRepository)
    {
        _leaveTypeRepository = leaveTypeRepository;
    }

    public async Task EnsureLeaveTypeExistsAsync(
        Guid leaveTypeId,
        CancellationToken cancellationToken = default)
    {
        var leaveType = await _leaveTypeRepository.GetByIdAsync(
            leaveTypeId,
            cancellationToken);

        if (leaveType is null)
        {
            throw new NotFoundException(
                "Leave Type",
                leaveTypeId);
        }
    }

    public async Task EnsureLeaveTypeCodeUniqueAsync(
        Guid companyId,
        string code,
        CancellationToken cancellationToken = default)
    {
        var exists = await _leaveTypeRepository.AnyAsync(
            x =>
                x.CompanyId == companyId &&
                x.Code == code &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Leave type code already exists for this company.");
        }
    }

    public async Task EnsureLeaveTypeCodeUniqueAsync(
        Guid companyId,
        string code,
        Guid leaveTypeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _leaveTypeRepository.AnyAsync(
            x =>
                x.CompanyId == companyId &&
                x.Code == code &&
                x.Id != leaveTypeId &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Leave type code already exists for this company.");
        }
    }
}
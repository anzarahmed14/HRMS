using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies.BusinessRules;

public class LeavePolicyBusinessRules
{
    private readonly IReadRepository<LeavePolicy, Guid> _leavePolicyRepository;

    public LeavePolicyBusinessRules(
        IReadRepository<LeavePolicy, Guid> leavePolicyRepository)
    {
        _leavePolicyRepository = leavePolicyRepository;
    }

    public async Task EnsureLeavePolicyExistsAsync(
        Guid leavePolicyId,
        CancellationToken cancellationToken = default)
    {
        var policy = await _leavePolicyRepository.GetByIdAsync(
            leavePolicyId,
            cancellationToken);

        if (policy is null)
        {
            throw new NotFoundException(
                "Leave Policy",
                leavePolicyId);
        }
    }

    public async Task EnsureLeavePolicyCodeUniqueAsync(
        Guid companyId,
        string code,
        CancellationToken cancellationToken = default)
    {
        var exists = await _leavePolicyRepository.AnyAsync(
            x =>
                x.CompanyId == companyId &&
                x.Code == code &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Leave policy code already exists for this company.");
        }
    }

    public async Task EnsureLeavePolicyCodeUniqueAsync(
        Guid companyId,
        string code,
        Guid leavePolicyId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _leavePolicyRepository.AnyAsync(
            x =>
                x.CompanyId == companyId &&
                x.Code == code &&
                x.Id != leavePolicyId &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Leave policy code already exists for this company.");
        }
    }
}
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using HRMS.Modules.Companies.Domain.Entities;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.BusinessRules;

public class AttendancePolicyBusinessRules
{
    private readonly IReadRepository<AttendancePolicy, Guid>
        _attendancePolicyReadRepository;

    private readonly IReadRepository<Company, Guid>
        _companyReadRepository;

    public AttendancePolicyBusinessRules(
        IReadRepository<AttendancePolicy, Guid> attendancePolicyReadRepository,
        IReadRepository<Company, Guid> companyReadRepository)
    {
        _attendancePolicyReadRepository = attendancePolicyReadRepository;
        _companyReadRepository = companyReadRepository;
    }

    public async Task EnsureCompanyExistsAsync(
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        var company = await _companyReadRepository.GetByIdAsync(
            companyId,
            cancellationToken);

        if (company is null)
        {
            throw new NotFoundException(
                "Company",
                companyId);
        }
    }

    public async Task EnsurePolicyCodeUniqueAsync(
        Guid companyId,
        string code,
        Guid? policyId = null,
        CancellationToken cancellationToken = default)
    {
        var exists = await _attendancePolicyReadRepository.AnyAsync(
            x =>
                x.CompanyId == companyId &&
                x.Code == code &&
                !x.IsDeleted &&
                (!policyId.HasValue || x.Id != policyId.Value),
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Attendance policy code already exists for this company.");
        }
    }

    public async Task EnsureDefaultPolicyUniqueAsync(
        Guid companyId,
        Guid? policyId = null,
        CancellationToken cancellationToken = default)
    {
        var exists = await _attendancePolicyReadRepository.AnyAsync(
            x =>
                x.CompanyId == companyId &&
                x.IsDefault &&
                !x.IsDeleted &&
                (!policyId.HasValue || x.Id != policyId.Value),
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Only one default attendance policy is allowed for a company.");
        }
    }
}
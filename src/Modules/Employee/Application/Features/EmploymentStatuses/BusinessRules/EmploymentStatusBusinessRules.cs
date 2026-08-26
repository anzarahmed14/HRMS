using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.EmploymentStatuses.BusinessRules;

public class EmploymentStatusBusinessRules
{
    private readonly IReadRepository<EmploymentStatus, Guid> _employmentStatusRepository;

    public EmploymentStatusBusinessRules(
        IReadRepository<EmploymentStatus, Guid> employmentStatusRepository)
    {
        _employmentStatusRepository = employmentStatusRepository;
    }

    public async Task EnsureEmploymentStatusExistsAsync(
        Guid employmentStatusId,
        CancellationToken cancellationToken = default)
    {
        var employmentStatus = await _employmentStatusRepository.GetByIdAsync(
            employmentStatusId,
            cancellationToken);

        if (employmentStatus is null)
        {
            throw new NotFoundException(
                "EmploymentStatus",
                employmentStatusId);
        }
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employmentStatusRepository.AnyAsync(
            x => x.Code == code,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Employment status code already exists.");
        }
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        Guid employmentStatusId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employmentStatusRepository.AnyAsync(
            x => x.Code == code &&
                 x.Id != employmentStatusId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Employment status code already exists.");
        }
    }

    public async Task EnsureNameUniqueAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employmentStatusRepository.AnyAsync(
            x => x.Name == name,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Employment status name already exists.");
        }
    }

    public async Task EnsureNameUniqueAsync(
        string name,
        Guid employmentStatusId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employmentStatusRepository.AnyAsync(
            x => x.Name == name &&
                 x.Id != employmentStatusId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Employment status name already exists.");
        }
    }
}

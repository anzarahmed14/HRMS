using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.EmploymentTypes.BusinessRules;

public class EmploymentTypeBusinessRules
{
    private readonly IReadRepository<EmploymentType, Guid> _employmentTypeRepository;

    public EmploymentTypeBusinessRules(
        IReadRepository<EmploymentType, Guid> employmentTypeRepository)
    {
        _employmentTypeRepository = employmentTypeRepository;
    }

    public async Task EnsureEmploymentTypeExistsAsync(
        Guid employmentTypeId,
        CancellationToken cancellationToken = default)
    {
        var employmentType = await _employmentTypeRepository.GetByIdAsync(
            employmentTypeId,
            cancellationToken);

        if (employmentType is null)
        {
            throw new NotFoundException(
                "EmploymentType",
                employmentTypeId);
        }
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employmentTypeRepository.AnyAsync(
            x => x.Code == code,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Employment type code already exists.");
        }
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        Guid employmentTypeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employmentTypeRepository.AnyAsync(
            x => x.Code == code &&
                 x.Id != employmentTypeId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Employment type code already exists.");
        }
    }

    public async Task EnsureNameUniqueAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employmentTypeRepository.AnyAsync(
            x => x.Name == name,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Employment type name already exists.");
        }
    }

    public async Task EnsureNameUniqueAsync(
        string name,
        Guid employmentTypeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employmentTypeRepository.AnyAsync(
            x => x.Name == name &&
                 x.Id != employmentTypeId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Employment type name already exists.");
        }
    }
}

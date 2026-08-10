using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using HRMS.Shared.Exceptions;

namespace HRMS.Application.Features.Departments.BusinessRules;

public class DepartmentBusinessRules
{
    private readonly IReadRepository<Department, Guid> _departmentReadRepository;

    public DepartmentBusinessRules(
        IReadRepository<Department, Guid> departmentReadRepository)
    {
        _departmentReadRepository = departmentReadRepository;
    }

    public async Task EnsureDepartmentExistsAsync(
        Guid departmentId,
        CancellationToken cancellationToken = default)
    {
        var department = await _departmentReadRepository.GetByIdAsync(
            departmentId,
            cancellationToken);

        if (department is null)
        {
            throw new NotFoundException(
                "Department",
                departmentId);
        }
    }

    public async Task EnsureDepartmentNameUniqueAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var exists = await _departmentReadRepository.AnyAsync(
            x => x.Name == name,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Department name already exists.");
        }
    }

    public async Task EnsureDepartmentNameUniqueAsync(
        string name,
        Guid departmentId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _departmentReadRepository.AnyAsync(
            x => x.Name == name &&
                 x.Id != departmentId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Department name already exists.");
        }
    }
}
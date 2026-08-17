using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;

namespace HRMS.Modules.Department.Application.Features.Departments.BusinessRules;

public class DepartmentBusinessRules
{
    private readonly IReadRepository<
        HRMS.Modules.Department.Domain.Entities.Department,
        Guid> _departmentReadRepository;

    public DepartmentBusinessRules(
        IReadRepository<
            HRMS.Modules.Department.Domain.Entities.Department,
            Guid> departmentReadRepository)
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
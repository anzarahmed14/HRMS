using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Application.Features.Nominees.BusinessRules;

public class EmployeeNomineeBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<EmployeeNominee, Guid> _nomineeRepository;
    private readonly IReadRepository<Relationship, Guid> _relationshipRepository;

    public EmployeeNomineeBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<EmployeeNominee, Guid> nomineeRepository,
        IReadRepository<Relationship, Guid> relationshipRepository)
    {
        _employeeRepository = employeeRepository;
        _nomineeRepository = nomineeRepository;
        _relationshipRepository = relationshipRepository;
    }

    public async Task EnsureEmployeeExistsAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(
            employeeId,
            cancellationToken);

        if (employee is null)
        {
            throw new NotFoundException(
                "Employee",
                employeeId);
        }
    }

    public async Task EnsureRelationshipExistsAsync(
        Guid relationshipId,
        CancellationToken cancellationToken = default)
    {
        var relationship = await _relationshipRepository.GetByIdAsync(
            relationshipId,
            cancellationToken);

        if (relationship is null)
        {
            throw new NotFoundException(
                "Relationship",
                relationshipId);
        }
    }

    public async Task EnsureNomineeNotDuplicateAsync(
        Guid employeeId,
        string name,
        Guid relationshipId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _nomineeRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.Name == name &&
                 x.RelationshipId == relationshipId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has this nominee.");
        }
    }

    public async Task EnsureNomineeNotDuplicateAsync(
        Guid employeeId,
        string name,
        Guid relationshipId,
        Guid nomineeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _nomineeRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.Name == name &&
                 x.RelationshipId == relationshipId &&
                 x.Id != nomineeId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has this nominee.");
        }
    }
}

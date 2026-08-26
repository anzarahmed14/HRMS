using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Application.Features.EmergencyContacts.BusinessRules;

public class EmergencyContactBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<EmergencyContact, Guid> _emergencyContactRepository;
    private readonly IReadRepository<Relationship, Guid> _relationshipRepository;

    public EmergencyContactBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<EmergencyContact, Guid> emergencyContactRepository,
        IReadRepository<Relationship, Guid> relationshipRepository)
    {
        _employeeRepository = employeeRepository;
        _emergencyContactRepository = emergencyContactRepository;
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

    public async Task EnsurePrimaryContactAvailableAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _emergencyContactRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.IsPrimary &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has a primary emergency contact.");
        }
    }

    public async Task EnsurePrimaryContactAvailableAsync(
        Guid employeeId,
        Guid emergencyContactId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _emergencyContactRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.IsPrimary &&
                 x.Id != emergencyContactId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has a primary emergency contact.");
        }
    }
}

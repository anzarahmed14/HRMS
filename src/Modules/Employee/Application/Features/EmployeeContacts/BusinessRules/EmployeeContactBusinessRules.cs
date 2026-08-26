using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.EmployeeContacts.BusinessRules;

public class EmployeeContactBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<EmployeeContact, Guid> _contactRepository;

    public EmployeeContactBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<EmployeeContact, Guid> contactRepository)
    {
        _employeeRepository = employeeRepository;
        _contactRepository = contactRepository;
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

    public async Task EnsureContactNotDuplicateAsync(
        Guid employeeId,
        string contactType,
        string contactValue,
        CancellationToken cancellationToken = default)
    {
        var exists = await _contactRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.ContactType == contactType &&
                 x.ContactValue == contactValue &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has this contact.");
        }
    }

    public async Task EnsureContactNotDuplicateAsync(
        Guid employeeId,
        string contactType,
        string contactValue,
        Guid contactId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _contactRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.ContactType == contactType &&
                 x.ContactValue == contactValue &&
                 x.Id != contactId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has this contact.");
        }
    }

    public async Task EnsurePrimaryContactAvailableAsync(
        Guid employeeId,
        string contactType,
        CancellationToken cancellationToken = default)
    {
        var exists = await _contactRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.ContactType == contactType &&
                 x.IsPrimary &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has a primary contact of this type.");
        }
    }

    public async Task EnsurePrimaryContactAvailableAsync(
        Guid employeeId,
        string contactType,
        Guid contactId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _contactRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.ContactType == contactType &&
                 x.IsPrimary &&
                 x.Id != contactId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has a primary contact of this type.");
        }
    }
}

using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Application.Features.GovernmentIdentifiers.BusinessRules;

public class EmployeeGovernmentIdentifierBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<IdentifierType, Guid> _identifierTypeRepository;
    private readonly IReadRepository<EmployeeGovernmentIdentifier, Guid> _identifierRepository;

    public EmployeeGovernmentIdentifierBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<IdentifierType, Guid> identifierTypeRepository,
        IReadRepository<EmployeeGovernmentIdentifier, Guid> identifierRepository)
    {
        _employeeRepository = employeeRepository;
        _identifierTypeRepository = identifierTypeRepository;
        _identifierRepository = identifierRepository;
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

    public async Task EnsureIdentifierTypeExistsAsync(
        Guid identifierTypeId,
        CancellationToken cancellationToken = default)
    {
        var identifierType = await _identifierTypeRepository.GetByIdAsync(
            identifierTypeId,
            cancellationToken);

        if (identifierType is null)
        {
            throw new NotFoundException(
                "IdentifierType",
                identifierTypeId);
        }
    }

    public async Task EnsureIdentifierTypeAvailableAsync(
        Guid employeeId,
        Guid identifierTypeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _identifierRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.IdentifierTypeId == identifierTypeId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has an identifier of this type.");
        }
    }

    public async Task EnsureIdentifierTypeAvailableAsync(
        Guid employeeId,
        Guid identifierTypeId,
        Guid identifierId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _identifierRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.IdentifierTypeId == identifierTypeId &&
                 x.Id != identifierId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has an identifier of this type.");
        }
    }
}

using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Application.Features.Dependents.BusinessRules;

public class EmployeeDependentBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<EmployeeDependent, Guid> _dependentRepository;
    private readonly IReadRepository<Relationship, Guid> _relationshipRepository;
    private readonly IReadRepository<Gender, Guid> _genderRepository;

    public EmployeeDependentBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<EmployeeDependent, Guid> dependentRepository,
        IReadRepository<Relationship, Guid> relationshipRepository,
        IReadRepository<Gender, Guid> genderRepository)
    {
        _employeeRepository = employeeRepository;
        _dependentRepository = dependentRepository;
        _relationshipRepository = relationshipRepository;
        _genderRepository = genderRepository;
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

    public async Task EnsureGenderExistsAsync(
        Guid genderId,
        CancellationToken cancellationToken = default)
    {
        var gender = await _genderRepository.GetByIdAsync(
            genderId,
            cancellationToken);

        if (gender is null)
        {
            throw new NotFoundException(
                "Gender",
                genderId);
        }
    }
}

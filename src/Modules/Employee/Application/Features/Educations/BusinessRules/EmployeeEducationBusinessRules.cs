using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.Educations.BusinessRules;

public class EmployeeEducationBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<EmployeeEducation, Guid> _educationRepository;

    public EmployeeEducationBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<EmployeeEducation, Guid> educationRepository)
    {
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
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

    public async Task EnsureHighestQualificationAvailableAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _educationRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.IsHighestQualification &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has a highest qualification.");
        }
    }

    public async Task EnsureHighestQualificationAvailableAsync(
        Guid employeeId,
        Guid educationId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _educationRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.IsHighestQualification &&
                 x.Id != educationId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has a highest qualification.");
        }
    }
}

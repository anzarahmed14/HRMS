using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using HRMS.Shared.Exceptions;

namespace HRMS.Application.Features.Employees.BusinessRules;

public class EmployeeBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<Department, Guid> _departmentRepository;

    public EmployeeBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<Department, Guid> departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
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

    public async Task EnsureEmployeeCodeUniqueAsync(
        string employeeCode,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeRepository.AnyAsync(
            x => x.EmployeeCode == employeeCode,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Employee code already exists.");
        }
    }

    public async Task EnsureEmployeeCodeUniqueAsync(
        string employeeCode,
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeRepository.AnyAsync(
            x => x.EmployeeCode == employeeCode &&
                 x.Id != employeeId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Employee code already exists.");
        }
    }

    public async Task EnsureEmailUniqueAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeRepository.AnyAsync(
            x => x.Email == email,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Employee email already exists.");
        }
    }

    public async Task EnsureEmailUniqueAsync(
        string email,
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeRepository.AnyAsync(
            x => x.Email == email &&
                 x.Id != employeeId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Employee email already exists.");
        }
    }

    public async Task EnsureDepartmentExistsAsync(
        Guid departmentId,
        CancellationToken cancellationToken = default)
    {
        var department = await _departmentRepository.GetByIdAsync(
            departmentId,
            cancellationToken);

        if (department is null)
        {
            throw new NotFoundException(
                "Department",
                departmentId);
        }
    }
}
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Department.Domain.Entities;
using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Application.Features.Employees.BusinessRules;

public class EmployeeBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<Department, Guid> _departmentRepository;
    private readonly IReadRepository<Gender, Guid> _genderRepository;
    private readonly IReadRepository<MaritalStatus, Guid> _maritalStatusRepository;

    public EmployeeBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<Department, Guid> departmentRepository,
        IReadRepository<Gender, Guid> genderRepository,
        IReadRepository<MaritalStatus, Guid> maritalStatusRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _genderRepository = genderRepository;
        _maritalStatusRepository = maritalStatusRepository;
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

    public async Task EnsureMaritalStatusExistsAsync(
        Guid maritalStatusId,
        CancellationToken cancellationToken = default)
    {
        var maritalStatus = await _maritalStatusRepository.GetByIdAsync(
            maritalStatusId,
            cancellationToken);

        if (maritalStatus is null)
        {
            throw new NotFoundException(
                "MaritalStatus",
                maritalStatusId);
        }
    }
}

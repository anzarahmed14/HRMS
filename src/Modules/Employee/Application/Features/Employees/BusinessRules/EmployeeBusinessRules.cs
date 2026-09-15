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
            throw new FluentValidation.ValidationException(
            [
                new FluentValidation.Results.ValidationFailure(
                    "Email",
                    "Email already exists.")
            ]);
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
            throw new FluentValidation.ValidationException(
            [
                new FluentValidation.Results.ValidationFailure(
                    "Email",
                    "Email already exists.")
            ]);
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

    // Validates a Reporting Manager being assigned to an EXISTING employee
    // (Update). Checks, in order: self-reference, existence (the query
    // filter on Employee already excludes soft-deleted rows, so a deleted
    // employee is reported as not found), and circular reporting.
    public async Task EnsureReportingManagerIsValidAsync(
        Guid employeeId,
        Guid reportingManagerId,
        CancellationToken cancellationToken = default)
    {
        if (employeeId == reportingManagerId)
        {
            throw new ConflictException(
                "An employee cannot be their own reporting manager.");
        }

        await EnsureEmployeeExistsAsync(
            reportingManagerId,
            cancellationToken);

        await EnsureNoCircularReportingAsync(
            employeeId,
            reportingManagerId,
            cancellationToken);
    }

    // Walks the proposed manager's reporting chain upward. If it ever
    // reaches back to the employee being updated, assigning
    // reportingManagerId would create a cycle in the hierarchy.
    private async Task EnsureNoCircularReportingAsync(
        Guid employeeId,
        Guid reportingManagerId,
        CancellationToken cancellationToken = default)
    {
        var visited = new HashSet<Guid>();
        Guid? currentId = reportingManagerId;

        while (currentId.HasValue)
        {
            if (currentId.Value == employeeId)
            {
                throw new ConflictException(
                    "Assigning this reporting manager would create a circular reporting relationship.");
            }

            if (!visited.Add(currentId.Value))
            {
                // Guards against a pre-existing cycle unrelated to this
                // update; stop walking rather than looping forever.
                break;
            }

            var manager = await _employeeRepository.GetByIdAsync(
                currentId.Value,
                cancellationToken);

            currentId = manager?.ReportingManagerId;
        }
    }
}

using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Application.Features.Certifications.BusinessRules;

public class EmployeeCertificationBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<Certification, Guid> _certificationRepository;
    private readonly IReadRepository<EmployeeCertification, Guid> _employeeCertificationRepository;

    public EmployeeCertificationBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<Certification, Guid> certificationRepository,
        IReadRepository<EmployeeCertification, Guid> employeeCertificationRepository)
    {
        _employeeRepository = employeeRepository;
        _certificationRepository = certificationRepository;
        _employeeCertificationRepository = employeeCertificationRepository;
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

    public async Task EnsureCertificationExistsAsync(
        Guid certificationId,
        CancellationToken cancellationToken = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(
            certificationId,
            cancellationToken);

        if (certification is null)
        {
            throw new NotFoundException(
                "Certification",
                certificationId);
        }
    }

    public async Task EnsureCertificationAvailableAsync(
        Guid employeeId,
        Guid certificationId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeCertificationRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.CertificationId == certificationId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has this certification.");
        }
    }

    public async Task EnsureCertificationAvailableAsync(
        Guid employeeId,
        Guid certificationId,
        Guid certificationIdToExclude,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeCertificationRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.CertificationId == certificationId &&
                 x.Id != certificationIdToExclude &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has this certification.");
        }
    }
}

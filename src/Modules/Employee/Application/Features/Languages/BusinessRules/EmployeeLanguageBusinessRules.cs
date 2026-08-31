using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Application.Features.Languages.BusinessRules;

public class EmployeeLanguageBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<Language, Guid> _languageRepository;
    private readonly IReadRepository<EmployeeLanguage, Guid> _employeeLanguageRepository;

    public EmployeeLanguageBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<Language, Guid> languageRepository,
        IReadRepository<EmployeeLanguage, Guid> employeeLanguageRepository)
    {
        _employeeRepository = employeeRepository;
        _languageRepository = languageRepository;
        _employeeLanguageRepository = employeeLanguageRepository;
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

    public async Task EnsureLanguageExistsAsync(
        Guid languageId,
        CancellationToken cancellationToken = default)
    {
        var language = await _languageRepository.GetByIdAsync(
            languageId,
            cancellationToken);

        if (language is null)
        {
            throw new NotFoundException(
                "Language",
                languageId);
        }
    }

    public async Task EnsureLanguageAvailableAsync(
        Guid employeeId,
        Guid languageId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeLanguageRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.LanguageId == languageId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has this language.");
        }
    }

    public async Task EnsureLanguageAvailableAsync(
        Guid employeeId,
        Guid languageId,
        Guid employeeLanguageId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeLanguageRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.LanguageId == languageId &&
                 x.Id != employeeLanguageId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has this language.");
        }
    }
}

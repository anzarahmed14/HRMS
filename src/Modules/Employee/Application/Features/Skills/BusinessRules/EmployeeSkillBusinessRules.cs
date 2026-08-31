using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Application.Features.Skills.BusinessRules;

public class EmployeeSkillBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<Skill, Guid> _skillRepository;
    private readonly IReadRepository<EmployeeSkill, Guid> _employeeSkillRepository;

    public EmployeeSkillBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<Skill, Guid> skillRepository,
        IReadRepository<EmployeeSkill, Guid> employeeSkillRepository)
    {
        _employeeRepository = employeeRepository;
        _skillRepository = skillRepository;
        _employeeSkillRepository = employeeSkillRepository;
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

    public async Task EnsureSkillExistsAsync(
        Guid skillId,
        CancellationToken cancellationToken = default)
    {
        var skill = await _skillRepository.GetByIdAsync(
            skillId,
            cancellationToken);

        if (skill is null)
        {
            throw new NotFoundException(
                "Skill",
                skillId);
        }
    }

    public async Task EnsureSkillAvailableAsync(
        Guid employeeId,
        Guid skillId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeSkillRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.SkillId == skillId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has this skill.");
        }
    }

    public async Task EnsureSkillAvailableAsync(
        Guid employeeId,
        Guid skillId,
        Guid employeeSkillId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeSkillRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.SkillId == skillId &&
                 x.Id != employeeSkillId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has this skill.");
        }
    }

    public async Task EnsurePrimarySkillAvailableAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeSkillRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.IsPrimary &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has a primary skill.");
        }
    }

    public async Task EnsurePrimarySkillAvailableAsync(
        Guid employeeId,
        Guid employeeSkillId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeSkillRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.IsPrimary &&
                 x.Id != employeeSkillId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has a primary skill.");
        }
    }
}

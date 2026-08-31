using HRMS.Application.Features.Skills.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Skills.Commands.UpdateEmployeeSkill;

public class UpdateEmployeeSkillCommandHandler
    : IRequestHandler<UpdateEmployeeSkillCommand>
{
    private readonly IReadRepository<EmployeeSkill, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeSkill, Guid> _writeRepository;
    private readonly EmployeeSkillBusinessRules _businessRules;

    public UpdateEmployeeSkillCommandHandler(
        IReadRepository<EmployeeSkill, Guid> readRepository,
        IWriteRepository<EmployeeSkill, Guid> writeRepository,
        EmployeeSkillBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateEmployeeSkillCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
            throw new InvalidOperationException(
                "Employee skill could not be loaded.");

        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureSkillExistsAsync(
            request.SkillId,
            cancellationToken);

        await _businessRules.EnsureSkillAvailableAsync(
            request.EmployeeId,
            request.SkillId,
            request.Id,
            cancellationToken);

        if (request.IsPrimary)
        {
            await _businessRules.EnsurePrimarySkillAvailableAsync(
                request.EmployeeId,
                request.Id,
                cancellationToken);
        }

        entity.ProficiencyLevel = request.ProficiencyLevel;
        entity.YearsOfExperience = request.YearsOfExperience;
        entity.IsPrimary = request.IsPrimary;
        entity.IsActive = request.IsActive;
        entity.EmployeeId = request.EmployeeId;
        entity.SkillId = request.SkillId;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}

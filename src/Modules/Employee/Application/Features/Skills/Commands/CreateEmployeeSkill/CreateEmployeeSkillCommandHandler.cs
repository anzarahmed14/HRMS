using HRMS.Application.Features.Skills.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Skills.Commands.CreateEmployeeSkill;

public class CreateEmployeeSkillCommandHandler
    : IRequestHandler<CreateEmployeeSkillCommand, Guid>
{
    private readonly IWriteRepository<EmployeeSkill, Guid> _writeRepository;
    private readonly EmployeeSkillBusinessRules _businessRules;

    public CreateEmployeeSkillCommandHandler(
        IWriteRepository<EmployeeSkill, Guid> writeRepository,
        EmployeeSkillBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmployeeSkillCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureSkillExistsAsync(
            request.SkillId,
            cancellationToken);

        await _businessRules.EnsureSkillAvailableAsync(
            request.EmployeeId,
            request.SkillId,
            cancellationToken);

        if (request.IsPrimary)
        {
            await _businessRules.EnsurePrimarySkillAvailableAsync(
                request.EmployeeId,
                cancellationToken);
        }

        var entity = new EmployeeSkill
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            SkillId = request.SkillId,
            ProficiencyLevel = request.ProficiencyLevel,
            YearsOfExperience = request.YearsOfExperience,
            IsPrimary = request.IsPrimary,
            IsActive = true
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}

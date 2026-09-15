using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Skills.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Skills.Commands.CreateSkill;

public class CreateSkillCommandHandler
    : IRequestHandler<CreateSkillCommand, Guid>
{
    private readonly IWriteRepository<Skill, Guid> _writeRepository;
    private readonly SkillBusinessRules _businessRules;

    public CreateSkillCommandHandler(
        IWriteRepository<Skill, Guid> writeRepository,
        SkillBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateSkillCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            name,
            cancellationToken);

        var entity = new Skill
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            Description = string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim(),
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}

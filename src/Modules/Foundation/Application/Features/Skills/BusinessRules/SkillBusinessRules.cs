using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Application.Features.Skills.BusinessRules;

public class SkillBusinessRules
{
    private readonly IReadRepository<Skill, Guid> _repository;

    public SkillBusinessRules(
        IReadRepository<Skill, Guid> repository)
    {
        _repository = repository;
    }

    public async Task EnsureExistsAsync(
        Guid skillId,
        CancellationToken cancellationToken = default)
    {
        var skill = await _repository.GetByIdAsync(
            skillId,
            cancellationToken);

        if (skill is null)
        {
            throw new NotFoundException(
                "Skill",
                skillId);
        }
    }
}

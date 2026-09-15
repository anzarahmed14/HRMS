using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Skills.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Skills.Commands.DeleteSkill;

public class DeleteSkillCommandHandler
    : IRequestHandler<DeleteSkillCommand>
{
    private readonly IReadRepository<Skill, Guid> _readRepository;
    private readonly IWriteRepository<Skill, Guid> _writeRepository;
    private readonly SkillBusinessRules _businessRules;

    public DeleteSkillCommandHandler(
        IReadRepository<Skill, Guid> readRepository,
        IWriteRepository<Skill, Guid> writeRepository,
        SkillBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteSkillCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            await _businessRules.EnsureSkillExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

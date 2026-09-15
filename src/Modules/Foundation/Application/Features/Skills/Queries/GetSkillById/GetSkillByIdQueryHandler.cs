using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Application.Features.Skills.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Skills.Queries.GetSkillById;

public class GetSkillByIdQueryHandler
    : IRequestHandler<GetSkillByIdQuery, SkillDto>
{
    private readonly IReadRepository<Skill, Guid> _repository;

    public GetSkillByIdQueryHandler(
        IReadRepository<Skill, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<SkillDto> Handle(
        GetSkillByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "Skill",
                request.Id);
        }

        return new SkillDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive
        };
    }
}

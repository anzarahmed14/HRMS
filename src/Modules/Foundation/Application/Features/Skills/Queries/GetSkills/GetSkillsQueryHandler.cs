using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Skills.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Skills.Queries.GetSkills;

public class GetSkillsQueryHandler
    : IRequestHandler<
        GetSkillsQuery,
        PagedResult<SkillDto>>
{
    private readonly IReadRepository<Skill, Guid> _repository;

    public GetSkillsQueryHandler(
        IReadRepository<Skill, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<SkillDto>> Handle(
        GetSkillsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<SkillDto>
        {
            Items = result.Items
                .Select(x => new SkillDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}

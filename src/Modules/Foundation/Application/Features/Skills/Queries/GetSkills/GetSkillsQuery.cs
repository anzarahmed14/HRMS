using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Skills.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Skills.Queries.GetSkills;

public record GetSkillsQuery(
    PagedRequest Request) : IRequest<PagedResult<SkillDto>>;

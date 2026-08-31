using HRMS.Application.Features.Skills.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.Skills.Queries.GetEmployeeSkills;

public sealed record GetEmployeeSkillsQuery(
    Guid EmployeeId,
    PagedRequest Request
) : IRequest<PagedResult<EmployeeSkillDto>>;

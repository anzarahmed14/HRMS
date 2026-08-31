using HRMS.Application.Features.Experiences.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.Experiences.Queries.GetEmployeeExperiences;

public sealed record GetEmployeeExperiencesQuery(
    Guid EmployeeId,
    PagedRequest Request
) : IRequest<PagedResult<EmployeeExperienceDto>>;

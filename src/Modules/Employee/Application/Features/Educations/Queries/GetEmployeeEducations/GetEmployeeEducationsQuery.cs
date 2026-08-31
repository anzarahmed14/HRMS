using HRMS.Application.Features.Educations.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.Educations.Queries.GetEmployeeEducations;

public sealed record GetEmployeeEducationsQuery(
    Guid EmployeeId,
    PagedRequest Request
) : IRequest<PagedResult<EmployeeEducationDto>>;

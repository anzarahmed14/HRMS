using HRMS.Application.Features.Languages.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.Languages.Queries.GetEmployeeLanguages;

public sealed record GetEmployeeLanguagesQuery(
    Guid EmployeeId,
    PagedRequest Request
) : IRequest<PagedResult<EmployeeLanguageDto>>;

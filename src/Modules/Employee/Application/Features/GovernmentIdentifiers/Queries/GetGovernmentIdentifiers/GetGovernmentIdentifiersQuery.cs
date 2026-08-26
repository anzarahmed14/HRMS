using HRMS.Application.Features.GovernmentIdentifiers.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.GovernmentIdentifiers.Queries.GetGovernmentIdentifiers;

public sealed record GetGovernmentIdentifiersQuery(
    Guid EmployeeId,
    PagedRequest Request
) : IRequest<PagedResult<GovernmentIdentifierDto>>;

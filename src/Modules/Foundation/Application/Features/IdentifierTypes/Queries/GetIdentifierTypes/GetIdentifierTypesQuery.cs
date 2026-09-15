using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.IdentifierTypes.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Queries.GetIdentifierTypes;

public record GetIdentifierTypesQuery(
    PagedRequest Request) : IRequest<PagedResult<IdentifierTypeDto>>;

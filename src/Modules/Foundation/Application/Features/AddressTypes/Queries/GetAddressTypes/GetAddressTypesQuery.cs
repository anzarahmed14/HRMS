using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.AddressTypes.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Queries.GetAddressTypes;

public sealed record GetAddressTypesQuery(
    PagedRequest Request) : IRequest<PagedResult<AddressTypeDto>>;

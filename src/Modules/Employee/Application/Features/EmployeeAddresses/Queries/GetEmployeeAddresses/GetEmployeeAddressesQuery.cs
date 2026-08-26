using HRMS.Application.Features.EmployeeAddresses.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.EmployeeAddresses.Queries.GetEmployeeAddresses;

public sealed record GetEmployeeAddressesQuery(
    Guid EmployeeId,
    PagedRequest Request
) : IRequest<PagedResult<EmployeeAddressDto>>;

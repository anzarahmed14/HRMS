using HRMS.Application.Features.EmployeeContacts.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.EmployeeContacts.Queries.GetEmployeeContacts;

public sealed record GetEmployeeContactsQuery(
    Guid EmployeeId,
    PagedRequest Request
) : IRequest<PagedResult<EmployeeContactDto>>;

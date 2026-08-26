using HRMS.Application.Features.EmergencyContacts.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.EmergencyContacts.Queries.GetEmergencyContacts;

public sealed record GetEmergencyContactsQuery(
    Guid EmployeeId,
    PagedRequest Request
) : IRequest<PagedResult<EmergencyContactDto>>;

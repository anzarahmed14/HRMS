using HRMS.Application.Features.EmergencyContacts.DTOs;
using MediatR;

namespace HRMS.Application.Features.EmergencyContacts.Queries.GetEmergencyContactById;

public record GetEmergencyContactByIdQuery(Guid Id)
    : IRequest<EmergencyContactDto?>;

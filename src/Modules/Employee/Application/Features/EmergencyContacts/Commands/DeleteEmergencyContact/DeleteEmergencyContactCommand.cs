using MediatR;

namespace HRMS.Application.Features.EmergencyContacts.Commands.DeleteEmergencyContact;

public record DeleteEmergencyContactCommand(Guid Id) : IRequest;

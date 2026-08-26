using MediatR;

namespace HRMS.Application.Features.EmployeeContacts.Commands.DeleteEmployeeContact;

public record DeleteEmployeeContactCommand(Guid Id) : IRequest;

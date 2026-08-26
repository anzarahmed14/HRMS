using MediatR;

namespace HRMS.Application.Features.EmployeeAddresses.Commands.DeleteEmployeeAddress;

public record DeleteEmployeeAddressCommand(Guid Id) : IRequest;

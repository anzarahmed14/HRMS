using MediatR;

namespace HRMS.Application.Features.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(Guid Id) : IRequest;
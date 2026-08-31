using MediatR;

namespace HRMS.Application.Features.Dependents.Commands.DeleteEmployeeDependent;

public record DeleteEmployeeDependentCommand(Guid Id) : IRequest;

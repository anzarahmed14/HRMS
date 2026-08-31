using MediatR;

namespace HRMS.Application.Features.Nominees.Commands.DeleteEmployeeNominee;

public record DeleteEmployeeNomineeCommand(Guid Id) : IRequest;

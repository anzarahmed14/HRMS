using MediatR;

namespace HRMS.Application.Features.EmploymentStatuses.Commands.DeleteEmploymentStatus;

public record DeleteEmploymentStatusCommand(Guid Id) : IRequest;

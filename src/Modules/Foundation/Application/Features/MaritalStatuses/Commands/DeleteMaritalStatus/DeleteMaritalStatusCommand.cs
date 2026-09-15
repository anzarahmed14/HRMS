using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Commands.DeleteMaritalStatus;

public record DeleteMaritalStatusCommand(
    Guid Id) : IRequest;

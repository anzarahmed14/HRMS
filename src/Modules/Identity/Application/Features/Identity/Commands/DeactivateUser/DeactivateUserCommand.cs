using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.DeactivateUser;

public record DeactivateUserCommand(Guid Id) : IRequest;

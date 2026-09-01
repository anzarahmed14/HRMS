using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.DeactivateRole;

public record DeactivateRoleCommand(Guid Id) : IRequest;

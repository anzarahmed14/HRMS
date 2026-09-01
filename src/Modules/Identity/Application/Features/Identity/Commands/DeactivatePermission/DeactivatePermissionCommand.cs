using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.DeactivatePermission;

public record DeactivatePermissionCommand(Guid Id) : IRequest;

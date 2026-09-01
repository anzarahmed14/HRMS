using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.AssignRolePermission;

public record AssignRolePermissionCommand : IRequest<Guid>
{
    public Guid RoleId { get; init; }

    public Guid PermissionId { get; init; }
}

using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.RemoveRolePermission;

public record RemoveRolePermissionCommand : IRequest
{
    public Guid RoleId { get; init; }

    public Guid PermissionId { get; init; }
}

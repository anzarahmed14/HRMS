using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.RemoveUserRole;

public record RemoveUserRoleCommand : IRequest
{
    public Guid UserId { get; init; }

    public Guid RoleId { get; init; }
}

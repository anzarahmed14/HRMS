using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.AssignUserRole;

public record AssignUserRoleCommand : IRequest<Guid>
{
    public Guid UserId { get; init; }

    public Guid RoleId { get; init; }
}

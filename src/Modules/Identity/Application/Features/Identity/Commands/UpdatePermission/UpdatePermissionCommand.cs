using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.UpdatePermission;

public record UpdatePermissionCommand : IRequest
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; }
}

using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.CreatePermission;

public record CreatePermissionCommand : IRequest<Guid>
{
    public Guid ModuleId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}

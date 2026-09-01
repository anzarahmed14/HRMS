using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.CreatePermission;

public record CreatePermissionCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}

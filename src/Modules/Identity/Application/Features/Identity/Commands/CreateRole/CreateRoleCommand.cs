using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.CreateRole;

public record CreateRoleCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}

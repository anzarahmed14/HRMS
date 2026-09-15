using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Commands.CreateMaritalStatus;

public record CreateMaritalStatusCommand : IRequest<Guid>
{
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; } = true;
}

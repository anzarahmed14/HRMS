using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Commands.UpdateMaritalStatus;

public record UpdateMaritalStatusCommand : IRequest
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}

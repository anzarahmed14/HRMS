using MediatR;

namespace HRMS.Application.Features.EmploymentStatuses.Commands.UpdateEmploymentStatus;

public record UpdateEmploymentStatusCommand : IRequest
{
    public Guid Id { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; }
}

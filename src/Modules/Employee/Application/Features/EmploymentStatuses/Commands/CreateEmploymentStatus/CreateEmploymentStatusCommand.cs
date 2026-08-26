using MediatR;

namespace HRMS.Application.Features.EmploymentStatuses.Commands.CreateEmploymentStatus;

public record CreateEmploymentStatusCommand : IRequest<Guid>
{
    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; } = true;
}

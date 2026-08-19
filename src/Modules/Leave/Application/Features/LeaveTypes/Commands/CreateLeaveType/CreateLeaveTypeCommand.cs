using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveTypes.Commands.CreateLeaveType;

public record CreateLeaveTypeCommand : IRequest<Guid>
{
    public Guid CompanyId { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsPaid { get; init; }

    public bool IsActive { get; init; } = true;
}
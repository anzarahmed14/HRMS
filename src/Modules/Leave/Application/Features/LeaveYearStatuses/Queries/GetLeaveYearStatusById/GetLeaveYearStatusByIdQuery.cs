using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Queries.GetLeaveYearStatusById;

public record GetLeaveYearStatusByIdQuery(Guid Id)
    : IRequest<LeaveYearStatusDto>;

public record LeaveYearStatusDto
{
    public Guid Id { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}

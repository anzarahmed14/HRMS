using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYears.Commands.CreateLeaveYear;

public record CreateLeaveYearCommand : IRequest<Guid>
{
    public Guid CompanyId { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public DateOnly StartDate { get; init; }

    public DateOnly EndDate { get; init; }

    public Guid StatusId { get; init; }
}
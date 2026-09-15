using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.Commands.CreateLeaveDayPart;

public record CreateLeaveDayPartCommand : IRequest<Guid>
{
    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public decimal DaysValue { get; init; }

    public bool IsActive { get; init; }
}

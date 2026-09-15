using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.Commands.UpdateLeaveDayPart;

public record UpdateLeaveDayPartCommand : IRequest
{
    public Guid Id { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public decimal DaysValue { get; init; }

    public bool IsActive { get; init; }
}

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.DTOs;

public record LeaveDayPartDto
{
    public Guid Id { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public decimal DaysValue { get; init; }

    public bool IsActive { get; init; }
}

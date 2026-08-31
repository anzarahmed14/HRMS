using MediatR;

namespace HRMS.Application.Features.Experiences.Commands.UpdateEmployeeExperience;

public record UpdateEmployeeExperienceCommand : IRequest
{
    public Guid Id { get; init; }

    public Guid EmployeeId { get; init; }

    public string CompanyName { get; init; } = string.Empty;

    public string JobTitle { get; init; } = string.Empty;

    public string EmploymentType { get; init; } = string.Empty;

    public DateOnly StartDate { get; init; }

    public DateOnly? EndDate { get; init; }

    public string? Location { get; init; }

    public string? Responsibilities { get; init; }
}

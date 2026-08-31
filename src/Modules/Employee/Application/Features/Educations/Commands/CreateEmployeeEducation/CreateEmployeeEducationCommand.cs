using MediatR;

namespace HRMS.Application.Features.Educations.Commands.CreateEmployeeEducation;

public record CreateEmployeeEducationCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; init; }

    public string EducationLevel { get; init; } = string.Empty;

    public string Qualification { get; init; } = string.Empty;

    public string? Specialization { get; init; }

    public string InstitutionName { get; init; } = string.Empty;

    public string? UniversityName { get; init; }

    public DateOnly? StartDate { get; init; }

    public DateOnly? EndDate { get; init; }

    public string? Grade { get; init; }

    public bool IsHighestQualification { get; init; }
}

using MediatR;

namespace HRMS.Application.Features.Skills.Commands.UpdateEmployeeSkill;

public record UpdateEmployeeSkillCommand : IRequest
{
    public Guid Id { get; init; }

    public Guid EmployeeId { get; init; }

    public Guid SkillId { get; init; }

    public string ProficiencyLevel { get; init; } = string.Empty;

    public decimal YearsOfExperience { get; init; }

    public bool IsPrimary { get; init; }

    public bool IsActive { get; init; } = true;
}

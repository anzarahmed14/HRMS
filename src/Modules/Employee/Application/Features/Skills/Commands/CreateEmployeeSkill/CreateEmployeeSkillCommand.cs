using MediatR;

namespace HRMS.Application.Features.Skills.Commands.CreateEmployeeSkill;

public record CreateEmployeeSkillCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; init; }

    public Guid SkillId { get; init; }

    public string ProficiencyLevel { get; init; } = string.Empty;

    public decimal YearsOfExperience { get; init; }

    public bool IsPrimary { get; init; }
}

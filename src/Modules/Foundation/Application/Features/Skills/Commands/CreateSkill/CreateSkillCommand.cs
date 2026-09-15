using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Skills.Commands.CreateSkill;

public record CreateSkillCommand : IRequest<Guid>
{
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; } = true;
}

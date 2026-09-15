using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Skills.Commands.DeleteSkill;

public record DeleteSkillCommand(
    Guid Id) : IRequest;

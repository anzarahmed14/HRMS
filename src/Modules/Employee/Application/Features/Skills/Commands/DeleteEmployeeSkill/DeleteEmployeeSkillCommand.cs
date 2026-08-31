using MediatR;

namespace HRMS.Application.Features.Skills.Commands.DeleteEmployeeSkill;

public record DeleteEmployeeSkillCommand(Guid Id) : IRequest;

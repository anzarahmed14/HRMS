using HRMS.Modules.Foundation.Application.Features.Skills.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Skills.Queries.GetSkillById;

public record GetSkillByIdQuery(
    Guid Id) : IRequest<SkillDto>;

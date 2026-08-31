using HRMS.Application.Features.Skills.DTOs;
using MediatR;

namespace HRMS.Application.Features.Skills.Queries.GetEmployeeSkillById;

public record GetEmployeeSkillByIdQuery(Guid Id)
    : IRequest<EmployeeSkillDto?>;

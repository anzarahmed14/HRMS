using HRMS.Application.Features.Skills.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Skills.Queries.GetEmployeeSkillById;

public class GetEmployeeSkillByIdQueryHandler
    : IRequestHandler<GetEmployeeSkillByIdQuery, EmployeeSkillDto?>
{
    private readonly IReadRepository<EmployeeSkill, Guid> _repository;

    public GetEmployeeSkillByIdQueryHandler(
        IReadRepository<EmployeeSkill, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeSkillDto?> Handle(
        GetEmployeeSkillByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
            return null;

        return new EmployeeSkillDto
        {
            Id = entity.Id,
            EmployeeId = entity.EmployeeId,
            SkillId = entity.SkillId,
            ProficiencyLevel = entity.ProficiencyLevel,
            YearsOfExperience = entity.YearsOfExperience,
            IsPrimary = entity.IsPrimary,
            IsActive = entity.IsActive
        };
    }
}

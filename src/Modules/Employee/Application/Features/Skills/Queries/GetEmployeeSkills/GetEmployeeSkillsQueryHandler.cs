using HRMS.Application.Features.Skills.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Skills.Queries.GetEmployeeSkills;

public sealed class GetEmployeeSkillsQueryHandler
    : IRequestHandler<
        GetEmployeeSkillsQuery,
        PagedResult<EmployeeSkillDto>>
{
    private readonly IReadRepository<EmployeeSkill, Guid> _repository;

    public GetEmployeeSkillsQueryHandler(
        IReadRepository<EmployeeSkill, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeSkillDto>> Handle(
        GetEmployeeSkillsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.EmployeeId == request.EmployeeId,
            cancellationToken: cancellationToken);

        return new PagedResult<EmployeeSkillDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmployeeSkillDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    SkillId = x.SkillId,
                    ProficiencyLevel = x.ProficiencyLevel,
                    YearsOfExperience = x.YearsOfExperience,
                    IsPrimary = x.IsPrimary,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}

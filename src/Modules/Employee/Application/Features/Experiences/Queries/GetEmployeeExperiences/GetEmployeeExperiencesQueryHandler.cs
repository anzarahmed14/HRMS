using HRMS.Application.Features.Experiences.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Experiences.Queries.GetEmployeeExperiences;

public sealed class GetEmployeeExperiencesQueryHandler
    : IRequestHandler<
        GetEmployeeExperiencesQuery,
        PagedResult<EmployeeExperienceDto>>
{
    private readonly IReadRepository<EmployeeExperience, Guid> _repository;

    public GetEmployeeExperiencesQueryHandler(
        IReadRepository<EmployeeExperience, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeExperienceDto>> Handle(
        GetEmployeeExperiencesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.EmployeeId == request.EmployeeId,
            cancellationToken: cancellationToken);

        return new PagedResult<EmployeeExperienceDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmployeeExperienceDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    CompanyName = x.CompanyName,
                    JobTitle = x.JobTitle,
                    EmploymentType = x.EmploymentType,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Location = x.Location,
                    Responsibilities = x.Responsibilities
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}

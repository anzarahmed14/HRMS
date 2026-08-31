using HRMS.Application.Features.Experiences.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Experiences.Queries.GetEmployeeExperienceById;

public class GetEmployeeExperienceByIdQueryHandler
    : IRequestHandler<GetEmployeeExperienceByIdQuery, EmployeeExperienceDto?>
{
    private readonly IReadRepository<EmployeeExperience, Guid> _repository;

    public GetEmployeeExperienceByIdQueryHandler(
        IReadRepository<EmployeeExperience, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeExperienceDto?> Handle(
        GetEmployeeExperienceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
            return null;

        return new EmployeeExperienceDto
        {
            Id = entity.Id,
            EmployeeId = entity.EmployeeId,
            CompanyName = entity.CompanyName,
            JobTitle = entity.JobTitle,
            EmploymentType = entity.EmploymentType,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Location = entity.Location,
            Responsibilities = entity.Responsibilities
        };
    }
}

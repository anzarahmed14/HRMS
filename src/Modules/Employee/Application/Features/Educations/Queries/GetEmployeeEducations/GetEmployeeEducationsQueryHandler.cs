using HRMS.Application.Features.Educations.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Educations.Queries.GetEmployeeEducations;

public sealed class GetEmployeeEducationsQueryHandler
    : IRequestHandler<
        GetEmployeeEducationsQuery,
        PagedResult<EmployeeEducationDto>>
{
    private readonly IReadRepository<EmployeeEducation, Guid> _repository;

    public GetEmployeeEducationsQueryHandler(
        IReadRepository<EmployeeEducation, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeEducationDto>> Handle(
        GetEmployeeEducationsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.EmployeeId == request.EmployeeId,
            cancellationToken: cancellationToken);

        return new PagedResult<EmployeeEducationDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmployeeEducationDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EducationLevel = x.EducationLevel,
                    Qualification = x.Qualification,
                    Specialization = x.Specialization,
                    InstitutionName = x.InstitutionName,
                    UniversityName = x.UniversityName,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Grade = x.Grade,
                    IsHighestQualification = x.IsHighestQualification,
                    IsVerified = x.IsVerified,
                    VerifiedOn = x.VerifiedOn
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}

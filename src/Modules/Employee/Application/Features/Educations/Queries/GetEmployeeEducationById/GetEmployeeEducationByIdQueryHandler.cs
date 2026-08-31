using HRMS.Application.Features.Educations.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Educations.Queries.GetEmployeeEducationById;

public class GetEmployeeEducationByIdQueryHandler
    : IRequestHandler<GetEmployeeEducationByIdQuery, EmployeeEducationDto?>
{
    private readonly IReadRepository<EmployeeEducation, Guid> _repository;

    public GetEmployeeEducationByIdQueryHandler(
        IReadRepository<EmployeeEducation, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeEducationDto?> Handle(
        GetEmployeeEducationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
            return null;

        return new EmployeeEducationDto
        {
            Id = entity.Id,
            EmployeeId = entity.EmployeeId,
            EducationLevel = entity.EducationLevel,
            Qualification = entity.Qualification,
            Specialization = entity.Specialization,
            InstitutionName = entity.InstitutionName,
            UniversityName = entity.UniversityName,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Grade = entity.Grade,
            IsHighestQualification = entity.IsHighestQualification,
            IsVerified = entity.IsVerified,
            VerifiedOn = entity.VerifiedOn
        };
    }
}

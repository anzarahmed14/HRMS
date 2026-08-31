using HRMS.Application.Features.Educations.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Educations.Commands.CreateEmployeeEducation;

public class CreateEmployeeEducationCommandHandler
    : IRequestHandler<CreateEmployeeEducationCommand, Guid>
{
    private readonly IWriteRepository<EmployeeEducation, Guid> _writeRepository;
    private readonly EmployeeEducationBusinessRules _businessRules;

    public CreateEmployeeEducationCommandHandler(
        IWriteRepository<EmployeeEducation, Guid> writeRepository,
        EmployeeEducationBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmployeeEducationCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        if (request.IsHighestQualification)
        {
            await _businessRules.EnsureHighestQualificationAvailableAsync(
                request.EmployeeId,
                cancellationToken);
        }

        var education = new EmployeeEducation
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            EducationLevel = request.EducationLevel,
            Qualification = request.Qualification,
            Specialization = request.Specialization,
            InstitutionName = request.InstitutionName,
            UniversityName = request.UniversityName,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Grade = request.Grade,
            IsHighestQualification = request.IsHighestQualification,
            IsVerified = false,
            VerifiedOn = null
        };

        await _writeRepository.AddAsync(
            education,
            cancellationToken);

        return education.Id;
    }
}

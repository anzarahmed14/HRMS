using HRMS.Application.Features.Experiences.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Experiences.Commands.CreateEmployeeExperience;

public class CreateEmployeeExperienceCommandHandler
    : IRequestHandler<CreateEmployeeExperienceCommand, Guid>
{
    private readonly IWriteRepository<EmployeeExperience, Guid> _writeRepository;
    private readonly EmployeeExperienceBusinessRules _businessRules;

    public CreateEmployeeExperienceCommandHandler(
        IWriteRepository<EmployeeExperience, Guid> writeRepository,
        EmployeeExperienceBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmployeeExperienceCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        var experience = new EmployeeExperience
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            CompanyName = request.CompanyName,
            JobTitle = request.JobTitle,
            EmploymentType = request.EmploymentType,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Location = request.Location,
            Responsibilities = request.Responsibilities
        };

        await _writeRepository.AddAsync(
            experience,
            cancellationToken);

        return experience.Id;
    }
}

using HRMS.Application.Features.EmploymentTypes.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmploymentTypes.Commands.CreateEmploymentType;

public class CreateEmploymentTypeCommandHandler
    : IRequestHandler<CreateEmploymentTypeCommand, Guid>
{
    private readonly IWriteRepository<EmploymentType, Guid> _writeRepository;
    private readonly EmploymentTypeBusinessRules _businessRules;

    public CreateEmploymentTypeCommandHandler(
        IWriteRepository<EmploymentType, Guid> writeRepository,
        EmploymentTypeBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmploymentTypeCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureCodeUniqueAsync(
            request.Code,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            request.Name,
            cancellationToken);

        var employmentType = new EmploymentType
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            employmentType,
            cancellationToken);

        return employmentType.Id;
    }
}

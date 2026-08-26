using HRMS.Application.Features.EmploymentStatuses.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmploymentStatuses.Commands.CreateEmploymentStatus;

public class CreateEmploymentStatusCommandHandler
    : IRequestHandler<CreateEmploymentStatusCommand, Guid>
{
    private readonly IWriteRepository<EmploymentStatus, Guid> _writeRepository;
    private readonly EmploymentStatusBusinessRules _businessRules;

    public CreateEmploymentStatusCommandHandler(
        IWriteRepository<EmploymentStatus, Guid> writeRepository,
        EmploymentStatusBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmploymentStatusCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureCodeUniqueAsync(
            request.Code,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            request.Name,
            cancellationToken);

        var employmentStatus = new EmploymentStatus
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            employmentStatus,
            cancellationToken);

        return employmentStatus.Id;
    }
}

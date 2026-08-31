using HRMS.Application.Features.Nominees.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Nominees.Commands.CreateEmployeeNominee;

public class CreateEmployeeNomineeCommandHandler
    : IRequestHandler<CreateEmployeeNomineeCommand, Guid>
{
    private readonly IWriteRepository<EmployeeNominee, Guid> _writeRepository;
    private readonly EmployeeNomineeBusinessRules _businessRules;

    public CreateEmployeeNomineeCommandHandler(
        IWriteRepository<EmployeeNominee, Guid> writeRepository,
        EmployeeNomineeBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmployeeNomineeCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureRelationshipExistsAsync(
            request.RelationshipId,
            cancellationToken);

        await _businessRules.EnsureNomineeNotDuplicateAsync(
            request.EmployeeId,
            request.Name,
            request.RelationshipId,
            cancellationToken);

        var nominee = new EmployeeNominee
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            Name = request.Name,
            RelationshipId = request.RelationshipId,
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            IsMinor = request.IsMinor,
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            nominee,
            cancellationToken);

        return nominee.Id;
    }
}

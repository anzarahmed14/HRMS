using HRMS.Application.Features.Dependents.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Dependents.Commands.CreateEmployeeDependent;

public class CreateEmployeeDependentCommandHandler
    : IRequestHandler<CreateEmployeeDependentCommand, Guid>
{
    private readonly IWriteRepository<EmployeeDependent, Guid> _writeRepository;
    private readonly EmployeeDependentBusinessRules _businessRules;

    public CreateEmployeeDependentCommandHandler(
        IWriteRepository<EmployeeDependent, Guid> writeRepository,
        EmployeeDependentBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmployeeDependentCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureRelationshipExistsAsync(
            request.RelationshipId,
            cancellationToken);

        await _businessRules.EnsureGenderExistsAsync(
            request.GenderId,
            cancellationToken);

        var dependent = new EmployeeDependent
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            Name = request.Name,
            RelationshipId = request.RelationshipId,
            GenderId = request.GenderId,
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            IsDependent = request.IsDependent,
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            dependent,
            cancellationToken);

        return dependent.Id;
    }
}

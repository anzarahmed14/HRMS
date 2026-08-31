using AutoMapper;
using HRMS.Application.Features.Dependents.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Dependents.Commands.UpdateEmployeeDependent;

public class UpdateEmployeeDependentCommandHandler
    : IRequestHandler<UpdateEmployeeDependentCommand>
{
    private readonly IReadRepository<EmployeeDependent, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeDependent, Guid> _writeRepository;
    private readonly EmployeeDependentBusinessRules _businessRules;
    private readonly IMapper _mapper;

    public UpdateEmployeeDependentCommandHandler(
        IReadRepository<EmployeeDependent, Guid> readRepository,
        IWriteRepository<EmployeeDependent, Guid> writeRepository,
        EmployeeDependentBusinessRules businessRules,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateEmployeeDependentCommand request,
        CancellationToken cancellationToken)
    {
        var dependent = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (dependent is null)
        {
            throw new InvalidOperationException(
                "Employee dependent could not be loaded.");
        }

        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureRelationshipExistsAsync(
            request.RelationshipId,
            cancellationToken);

        await _businessRules.EnsureGenderExistsAsync(
            request.GenderId,
            cancellationToken);

        _mapper.Map(request, dependent);

        await _writeRepository.UpdateAsync(
            dependent,
            cancellationToken);
    }
}

using HRMS.Application.Features.Dependents.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Dependents.Commands.DeleteEmployeeDependent;

public class DeleteEmployeeDependentCommandHandler
    : IRequestHandler<DeleteEmployeeDependentCommand>
{
    private readonly IReadRepository<EmployeeDependent, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeDependent, Guid> _writeRepository;
    private readonly EmployeeDependentBusinessRules _businessRules;

    public DeleteEmployeeDependentCommandHandler(
        IReadRepository<EmployeeDependent, Guid> readRepository,
        IWriteRepository<EmployeeDependent, Guid> writeRepository,
        EmployeeDependentBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteEmployeeDependentCommand request,
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
            dependent.EmployeeId,
            cancellationToken);

        await _writeRepository.DeleteAsync(
            dependent,
            cancellationToken);
    }
}

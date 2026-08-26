using HRMS.Application.Features.EmployeeContacts.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmployeeContacts.Commands.DeleteEmployeeContact;

public class DeleteEmployeeContactCommandHandler
    : IRequestHandler<DeleteEmployeeContactCommand>
{
    private readonly IReadRepository<EmployeeContact, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeContact, Guid> _writeRepository;
    private readonly EmployeeContactBusinessRules _businessRules;

    public DeleteEmployeeContactCommandHandler(
        IReadRepository<EmployeeContact, Guid> readRepository,
        IWriteRepository<EmployeeContact, Guid> writeRepository,
        EmployeeContactBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteEmployeeContactCommand request,
        CancellationToken cancellationToken)
    {
        var employeeContact = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employeeContact is null)
        {
            throw new InvalidOperationException(
                "Employee contact could not be loaded.");
        }

        await _businessRules.EnsureEmployeeExistsAsync(
            employeeContact.EmployeeId,
            cancellationToken);

        await _writeRepository.DeleteAsync(
            employeeContact,
            cancellationToken);
    }
}

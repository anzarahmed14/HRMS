using HRMS.Application.Features.EmployeeContacts.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmployeeContacts.Commands.CreateEmployeeContact;

public class CreateEmployeeContactCommandHandler
    : IRequestHandler<CreateEmployeeContactCommand, Guid>
{
    private readonly IWriteRepository<EmployeeContact, Guid> _writeRepository;
    private readonly EmployeeContactBusinessRules _businessRules;

    public CreateEmployeeContactCommandHandler(
        IWriteRepository<EmployeeContact, Guid> writeRepository,
        EmployeeContactBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmployeeContactCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureContactNotDuplicateAsync(
            request.EmployeeId,
            request.ContactType,
            request.ContactValue,
            cancellationToken);

        if (request.IsPrimary)
        {
            await _businessRules.EnsurePrimaryContactAvailableAsync(
                request.EmployeeId,
                request.ContactType,
                cancellationToken);
        }

        var employeeContact = new EmployeeContact
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            ContactType = request.ContactType,
            ContactValue = request.ContactValue,
            IsPrimary = request.IsPrimary
        };

        await _writeRepository.AddAsync(
            employeeContact,
            cancellationToken);

        return employeeContact.Id;
    }
}

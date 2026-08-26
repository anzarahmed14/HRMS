using AutoMapper;
using HRMS.Application.Features.EmployeeContacts.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmployeeContacts.Commands.UpdateEmployeeContact;

public class UpdateEmployeeContactCommandHandler
    : IRequestHandler<UpdateEmployeeContactCommand>
{
    private readonly IReadRepository<EmployeeContact, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeContact, Guid> _writeRepository;
    private readonly EmployeeContactBusinessRules _businessRules;
    private readonly IMapper _mapper;

    public UpdateEmployeeContactCommandHandler(
        IReadRepository<EmployeeContact, Guid> readRepository,
        IWriteRepository<EmployeeContact, Guid> writeRepository,
        EmployeeContactBusinessRules businessRules,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateEmployeeContactCommand request,
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
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureContactNotDuplicateAsync(
            request.EmployeeId,
            request.ContactType,
            request.ContactValue,
            request.Id,
            cancellationToken);

        if (request.IsPrimary)
        {
            await _businessRules.EnsurePrimaryContactAvailableAsync(
                request.EmployeeId,
                request.ContactType,
                request.Id,
                cancellationToken);
        }

        _mapper.Map(request, employeeContact);

        await _writeRepository.UpdateAsync(
            employeeContact,
            cancellationToken);
    }
}

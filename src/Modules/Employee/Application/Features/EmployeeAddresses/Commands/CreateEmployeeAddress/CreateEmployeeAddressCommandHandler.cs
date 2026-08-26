using HRMS.Application.Features.EmployeeAddresses.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmployeeAddresses.Commands.CreateEmployeeAddress;

public class CreateEmployeeAddressCommandHandler
    : IRequestHandler<CreateEmployeeAddressCommand, Guid>
{
    private readonly IWriteRepository<EmployeeAddress, Guid> _writeRepository;
    private readonly EmployeeAddressBusinessRules _businessRules;

    public CreateEmployeeAddressCommandHandler(
        IWriteRepository<EmployeeAddress, Guid> writeRepository,
        EmployeeAddressBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmployeeAddressCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureAddressTypeExistsAsync(
            request.AddressTypeId,
            cancellationToken);

        await _businessRules.EnsureCountryExistsAsync(
            request.CountryId,
            cancellationToken);

        await _businessRules.EnsureStateBelongsToCountryAsync(
            request.StateId,
            request.CountryId,
            cancellationToken);

        await _businessRules.EnsureAddressTypeAvailableAsync(
            request.EmployeeId,
            request.AddressTypeId,
            cancellationToken);

        if (request.IsPrimary)
        {
            await _businessRules.EnsurePrimaryAddressAvailableAsync(
                request.EmployeeId,
                cancellationToken);
        }

        var employeeAddress = new EmployeeAddress
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            AddressTypeId = request.AddressTypeId,
            CountryId = request.CountryId,
            StateId = request.StateId,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            City = request.City,
            PostalCode = request.PostalCode,
            IsPrimary = request.IsPrimary
        };

        await _writeRepository.AddAsync(
            employeeAddress,
            cancellationToken);

        return employeeAddress.Id;
    }
}

using AutoMapper;
using HRMS.Application.Features.EmployeeAddresses.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmployeeAddresses.Commands.UpdateEmployeeAddress;

public class UpdateEmployeeAddressCommandHandler
    : IRequestHandler<UpdateEmployeeAddressCommand>
{
    private readonly IReadRepository<EmployeeAddress, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeAddress, Guid> _writeRepository;
    private readonly EmployeeAddressBusinessRules _businessRules;
    private readonly IMapper _mapper;

    public UpdateEmployeeAddressCommandHandler(
        IReadRepository<EmployeeAddress, Guid> readRepository,
        IWriteRepository<EmployeeAddress, Guid> writeRepository,
        EmployeeAddressBusinessRules businessRules,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateEmployeeAddressCommand request,
        CancellationToken cancellationToken)
    {
        var employeeAddress = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employeeAddress is null)
        {
            throw new InvalidOperationException(
                "Employee address could not be loaded.");
        }

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
            request.Id,
            cancellationToken);

        if (request.IsPrimary)
        {
            await _businessRules.EnsurePrimaryAddressAvailableAsync(
                request.EmployeeId,
                request.Id,
                cancellationToken);
        }

        _mapper.Map(request, employeeAddress);

        await _writeRepository.UpdateAsync(
            employeeAddress,
            cancellationToken);
    }
}

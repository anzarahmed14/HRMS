using HRMS.Application.Features.EmployeeAddresses.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmployeeAddresses.Commands.DeleteEmployeeAddress;

public class DeleteEmployeeAddressCommandHandler
    : IRequestHandler<DeleteEmployeeAddressCommand>
{
    private readonly IReadRepository<EmployeeAddress, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeAddress, Guid> _writeRepository;
    private readonly EmployeeAddressBusinessRules _businessRules;

    public DeleteEmployeeAddressCommandHandler(
        IReadRepository<EmployeeAddress, Guid> readRepository,
        IWriteRepository<EmployeeAddress, Guid> writeRepository,
        EmployeeAddressBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteEmployeeAddressCommand request,
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
            employeeAddress.EmployeeId,
            cancellationToken);

        await _writeRepository.DeleteAsync(
            employeeAddress,
            cancellationToken);
    }
}

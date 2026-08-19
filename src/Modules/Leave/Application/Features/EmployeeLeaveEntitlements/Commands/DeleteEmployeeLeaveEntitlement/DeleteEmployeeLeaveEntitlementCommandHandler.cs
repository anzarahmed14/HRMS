using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.DeleteEmployeeLeaveEntitlement;

public class DeleteEmployeeLeaveEntitlementCommandHandler
    : IRequestHandler<DeleteEmployeeLeaveEntitlementCommand>
{
    private readonly IReadRepository<EmployeeLeaveEntitlement, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeLeaveEntitlement, Guid> _writeRepository;
    private readonly EmployeeLeaveEntitlementBusinessRules _businessRules;

    public DeleteEmployeeLeaveEntitlementCommandHandler(
        IReadRepository<EmployeeLeaveEntitlement, Guid> readRepository,
        IWriteRepository<EmployeeLeaveEntitlement, Guid> writeRepository,
        EmployeeLeaveEntitlementBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteEmployeeLeaveEntitlementCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
        {
            await _businessRules.EnsureEntitlementExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

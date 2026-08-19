using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Features.LeavePolicies.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies.Commands.UpdateLeavePolicy;

public class UpdateLeavePolicyCommandHandler
    : IRequestHandler<UpdateLeavePolicyCommand>
{
    private readonly IReadRepository<LeavePolicy, Guid> _readRepository;
    private readonly IWriteRepository<LeavePolicy, Guid> _writeRepository;
    private readonly LeavePolicyBusinessRules _businessRules;

    public UpdateLeavePolicyCommandHandler(
        IReadRepository<LeavePolicy, Guid> readRepository,
        IWriteRepository<LeavePolicy, Guid> writeRepository,
        LeavePolicyBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateLeavePolicyCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
        {
            await _businessRules.EnsureLeavePolicyExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        if (entity.CompanyId != request.CompanyId)
        {
            throw new ConflictException(
                "A leave policy cannot be moved to another company.");
        }

        await _businessRules.EnsureLeavePolicyCodeUniqueAsync(
            request.CompanyId,
            request.Code.Trim(),
            request.Id,
            cancellationToken);

        entity.Code = request.Code.Trim();
        entity.Name = request.Name.Trim();
        entity.Description = string.IsNullOrWhiteSpace(request.Description)
            ? null
            : request.Description.Trim();
        entity.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}

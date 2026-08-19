using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Features.LeaveTypes.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveTypes.Commands.UpdateLeaveType;

public class UpdateLeaveTypeCommandHandler
    : IRequestHandler<UpdateLeaveTypeCommand>
{
    private readonly IReadRepository<LeaveType, Guid> _readRepository;
    private readonly IWriteRepository<LeaveType, Guid> _writeRepository;
    private readonly LeaveTypeBusinessRules _businessRules;

    public UpdateLeaveTypeCommandHandler(
        IReadRepository<LeaveType, Guid> readRepository,
        IWriteRepository<LeaveType, Guid> writeRepository,
        LeaveTypeBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateLeaveTypeCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
        {
            await _businessRules.EnsureLeaveTypeExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        if (entity.CompanyId != request.CompanyId)
        {
            throw new ConflictException(
                "A leave type cannot be moved to another company.");
        }

        await _businessRules.EnsureLeaveTypeCodeUniqueAsync(
            request.CompanyId,
            request.Code.Trim(),
            request.Id,
            cancellationToken);

        entity.Code = request.Code.Trim();
        entity.Name = request.Name.Trim();
        entity.Description = string.IsNullOrWhiteSpace(request.Description)
            ? null
            : request.Description.Trim();
        entity.IsPaid = request.IsPaid;
        entity.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}

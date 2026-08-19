using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveTypes.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveTypes.Commands.DeleteLeaveType;

public class DeleteLeaveTypeCommandHandler
    : IRequestHandler<DeleteLeaveTypeCommand>
{
    private readonly IReadRepository<LeaveType, Guid> _readRepository;
    private readonly IWriteRepository<LeaveType, Guid> _writeRepository;
    private readonly LeaveTypeBusinessRules _businessRules;

    public DeleteLeaveTypeCommandHandler(
        IReadRepository<LeaveType, Guid> readRepository,
        IWriteRepository<LeaveType, Guid> writeRepository,
        LeaveTypeBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteLeaveTypeCommand request,
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

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

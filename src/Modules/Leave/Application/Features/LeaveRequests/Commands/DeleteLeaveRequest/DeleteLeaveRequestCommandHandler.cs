using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.DeleteLeaveRequest;

public sealed class DeleteLeaveRequestCommandHandler
    : IRequestHandler<DeleteLeaveRequestCommand>
{
    private readonly IReadRepository<LeaveRequest, Guid> _readRepository;
    private readonly IWriteRepository<LeaveRequest, Guid> _writeRepository;
    private readonly LeaveRequestBusinessRules _businessRules;

    public DeleteLeaveRequestCommandHandler(
        IReadRepository<LeaveRequest, Guid> readRepository,
        IWriteRepository<LeaveRequest, Guid> writeRepository,
        LeaveRequestBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteLeaveRequestCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Get leave request
        var leaveRequest = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (leaveRequest is null || leaveRequest.IsDeleted)
        {
            throw new NotFoundException(
                "Leave Request",
                request.Id);
        }

        // 2. Only DRAFT requests can be deleted
        await _businessRules.EnsureCanDeleteAsync(
            leaveRequest,
            cancellationToken);

        // 3. Soft delete
        await _writeRepository.DeleteAsync(
            leaveRequest,
            cancellationToken);
    }
}

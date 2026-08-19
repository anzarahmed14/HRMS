using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveYears.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYears.Commands.DeleteLeaveYear;

public class DeleteLeaveYearCommandHandler
    : IRequestHandler<DeleteLeaveYearCommand>
{
    private readonly IReadRepository<LeaveYear, Guid> _readRepository;
    private readonly IWriteRepository<LeaveYear, Guid> _writeRepository;
    private readonly LeaveYearBusinessRules _businessRules;

    public DeleteLeaveYearCommandHandler(
        IReadRepository<LeaveYear, Guid> readRepository,
        IWriteRepository<LeaveYear, Guid> writeRepository,
        LeaveYearBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteLeaveYearCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
        {
            await _businessRules.EnsureLeaveYearExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

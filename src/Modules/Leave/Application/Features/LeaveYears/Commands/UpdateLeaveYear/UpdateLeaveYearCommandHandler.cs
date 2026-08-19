using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Features.LeaveYears.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYears.Commands.UpdateLeaveYear;

public class UpdateLeaveYearCommandHandler
    : IRequestHandler<UpdateLeaveYearCommand>
{
    private readonly IReadRepository<LeaveYear, Guid> _readRepository;
    private readonly IWriteRepository<LeaveYear, Guid> _writeRepository;
    private readonly LeaveYearBusinessRules _businessRules;

    public UpdateLeaveYearCommandHandler(
        IReadRepository<LeaveYear, Guid> readRepository,
        IWriteRepository<LeaveYear, Guid> writeRepository,
        LeaveYearBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateLeaveYearCommand request,
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

        if (entity.CompanyId != request.CompanyId)
        {
            throw new ConflictException(
                "A leave year cannot be moved to another company.");
        }

        await _businessRules.EnsureLeaveYearDatesAreValidAsync(
            request.StartDate,
            request.EndDate);

        await _businessRules.EnsureNoOverlappingLeaveYearAsync(
            request.CompanyId,
            request.StartDate,
            request.EndDate,
            request.Id,
            cancellationToken);

        if (await _businessRules.IsActiveStatusAsync(
                request.StatusId,
                cancellationToken))
        {
            await _businessRules.EnsureOnlyOneActiveLeaveYearAsync(
                request.CompanyId,
                request.StatusId,
                request.Id,
                cancellationToken);
        }

        entity.Code = request.Code.Trim();
        entity.Name = request.Name.Trim();
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;
        entity.StatusId = request.StatusId;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}

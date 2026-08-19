using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveYears.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYears.Commands.CreateLeaveYear;

public class CreateLeaveYearCommandHandler
    : IRequestHandler<CreateLeaveYearCommand, Guid>
{
    private readonly IWriteRepository<LeaveYear, Guid>
        _writeRepository;

    private readonly LeaveYearBusinessRules
        _businessRules;

    public CreateLeaveYearCommandHandler(
        IWriteRepository<LeaveYear, Guid> writeRepository,
        LeaveYearBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateLeaveYearCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureLeaveYearDatesAreValidAsync(
            request.StartDate,
            request.EndDate);

        await _businessRules.EnsureNoOverlappingLeaveYearAsync(
            request.CompanyId,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        if (await _businessRules.IsActiveStatusAsync(
                request.StatusId,
                cancellationToken))
        {
            await _businessRules.EnsureOnlyOneActiveLeaveYearAsync(
                request.CompanyId,
                request.StatusId,
                cancellationToken);
        }

        var entity = new LeaveYear
        {
            CompanyId = request.CompanyId,
            Code = request.Code.Trim(),
            Name = request.Name.Trim(),
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            StatusId = request.StatusId
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}
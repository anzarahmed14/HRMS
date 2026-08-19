using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveTypes.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveTypes.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandHandler
    : IRequestHandler<CreateLeaveTypeCommand, Guid>
{
    private readonly IWriteRepository<LeaveType, Guid> _writeRepository;
    private readonly LeaveTypeBusinessRules _businessRules;

    public CreateLeaveTypeCommandHandler(
        IWriteRepository<LeaveType, Guid> writeRepository,
        LeaveTypeBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateLeaveTypeCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();

        await _businessRules.EnsureLeaveTypeCodeUniqueAsync(
            request.CompanyId,
            code,
            cancellationToken);

        var entity = new LeaveType
        {
            CompanyId = request.CompanyId,
            Code = code,
            Name = request.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim(),
            IsPaid = request.IsPaid,
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}
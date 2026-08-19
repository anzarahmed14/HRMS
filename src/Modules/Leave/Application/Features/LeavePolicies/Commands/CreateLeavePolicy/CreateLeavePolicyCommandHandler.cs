using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeavePolicies.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies.Commands.CreateLeavePolicy;

public class CreateLeavePolicyCommandHandler
    : IRequestHandler<CreateLeavePolicyCommand, Guid>
{
    private readonly IWriteRepository<LeavePolicy, Guid> _writeRepository;
    private readonly LeavePolicyBusinessRules _businessRules;

    public CreateLeavePolicyCommandHandler(
        IWriteRepository<LeavePolicy, Guid> writeRepository,
        LeavePolicyBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateLeavePolicyCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();

        await _businessRules.EnsureLeavePolicyCodeUniqueAsync(
            request.CompanyId,
            code,
            cancellationToken);

        var entity = new LeavePolicy
        {
            CompanyId = request.CompanyId,
            Code = code,
            Name = request.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim(),
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}
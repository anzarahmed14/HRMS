using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.MaritalStatuses.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Commands.CreateMaritalStatus;

public class CreateMaritalStatusCommandHandler
    : IRequestHandler<CreateMaritalStatusCommand, Guid>
{
    private readonly IWriteRepository<MaritalStatus, Guid> _writeRepository;
    private readonly MaritalStatusBusinessRules _businessRules;

    public CreateMaritalStatusCommandHandler(
        IWriteRepository<MaritalStatus, Guid> writeRepository,
        MaritalStatusBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateMaritalStatusCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken);

        var entity = new MaritalStatus
        {
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

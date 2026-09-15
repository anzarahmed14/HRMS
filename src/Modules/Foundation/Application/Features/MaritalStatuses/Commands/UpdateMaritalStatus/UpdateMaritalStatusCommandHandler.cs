using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.MaritalStatuses.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Commands.UpdateMaritalStatus;

public class UpdateMaritalStatusCommandHandler
    : IRequestHandler<UpdateMaritalStatusCommand>
{
    private readonly IReadRepository<MaritalStatus, Guid> _readRepository;
    private readonly IWriteRepository<MaritalStatus, Guid> _writeRepository;
    private readonly MaritalStatusBusinessRules _businessRules;

    public UpdateMaritalStatusCommandHandler(
        IReadRepository<MaritalStatus, Guid> readRepository,
        IWriteRepository<MaritalStatus, Guid> writeRepository,
        MaritalStatusBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateMaritalStatusCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            await _businessRules.EnsureMaritalStatusExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            request.Id,
            cancellationToken);

        entity.Code = code;
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

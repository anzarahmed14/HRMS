using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.IdentifierTypes.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Commands.UpdateIdentifierType;

public class UpdateIdentifierTypeCommandHandler
    : IRequestHandler<UpdateIdentifierTypeCommand>
{
    private readonly IReadRepository<IdentifierType, Guid> _readRepository;
    private readonly IWriteRepository<IdentifierType, Guid> _writeRepository;
    private readonly IdentifierTypeBusinessRules _businessRules;

    public UpdateIdentifierTypeCommandHandler(
        IReadRepository<IdentifierType, Guid> readRepository,
        IWriteRepository<IdentifierType, Guid> writeRepository,
        IdentifierTypeBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateIdentifierTypeCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            await _businessRules.EnsureIdentifierTypeExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            request.Id,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            name,
            request.Id,
            cancellationToken);

        entity.Code = code;
        entity.Name = name;
        entity.Description = string.IsNullOrWhiteSpace(request.Description)
            ? null
            : request.Description.Trim();
        entity.IsSensitive = request.IsSensitive;
        entity.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}

using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Relationships.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Relationships.Commands.UpdateRelationship;

public class UpdateRelationshipCommandHandler
    : IRequestHandler<UpdateRelationshipCommand>
{
    private readonly IReadRepository<Relationship, Guid> _readRepository;
    private readonly IWriteRepository<Relationship, Guid> _writeRepository;
    private readonly RelationshipBusinessRules _businessRules;

    public UpdateRelationshipCommandHandler(
        IReadRepository<Relationship, Guid> readRepository,
        IWriteRepository<Relationship, Guid> writeRepository,
        RelationshipBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateRelationshipCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            await _businessRules.EnsureRelationshipExistsAsync(
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

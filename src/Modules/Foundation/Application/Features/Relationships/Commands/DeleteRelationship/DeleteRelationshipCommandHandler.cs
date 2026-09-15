using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Relationships.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Relationships.Commands.DeleteRelationship;

public class DeleteRelationshipCommandHandler
    : IRequestHandler<DeleteRelationshipCommand>
{
    private readonly IReadRepository<Relationship, Guid> _readRepository;
    private readonly IWriteRepository<Relationship, Guid> _writeRepository;
    private readonly RelationshipBusinessRules _businessRules;

    public DeleteRelationshipCommandHandler(
        IReadRepository<Relationship, Guid> readRepository,
        IWriteRepository<Relationship, Guid> writeRepository,
        RelationshipBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteRelationshipCommand request,
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

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

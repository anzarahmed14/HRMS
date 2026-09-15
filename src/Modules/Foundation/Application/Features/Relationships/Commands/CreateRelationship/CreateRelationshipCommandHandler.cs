using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Relationships.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Relationships.Commands.CreateRelationship;

public class CreateRelationshipCommandHandler
    : IRequestHandler<CreateRelationshipCommand, Guid>
{
    private readonly IWriteRepository<Relationship, Guid> _writeRepository;
    private readonly RelationshipBusinessRules _businessRules;

    public CreateRelationshipCommandHandler(
        IWriteRepository<Relationship, Guid> writeRepository,
        RelationshipBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateRelationshipCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken);

        var entity = new Relationship
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

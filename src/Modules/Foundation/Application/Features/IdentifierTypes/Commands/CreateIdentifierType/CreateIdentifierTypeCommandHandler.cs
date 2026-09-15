using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.IdentifierTypes.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Commands.CreateIdentifierType;

public class CreateIdentifierTypeCommandHandler
    : IRequestHandler<CreateIdentifierTypeCommand, Guid>
{
    private readonly IWriteRepository<IdentifierType, Guid> _writeRepository;
    private readonly IdentifierTypeBusinessRules _businessRules;

    public CreateIdentifierTypeCommandHandler(
        IWriteRepository<IdentifierType, Guid> writeRepository,
        IdentifierTypeBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateIdentifierTypeCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            name,
            cancellationToken);

        var entity = new IdentifierType
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            Description = string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim(),
            IsSensitive = request.IsSensitive,
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}

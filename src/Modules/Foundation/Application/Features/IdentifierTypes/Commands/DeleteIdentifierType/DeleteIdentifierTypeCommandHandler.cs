using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.IdentifierTypes.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Commands.DeleteIdentifierType;

public class DeleteIdentifierTypeCommandHandler
    : IRequestHandler<DeleteIdentifierTypeCommand>
{
    private readonly IReadRepository<IdentifierType, Guid> _readRepository;
    private readonly IWriteRepository<IdentifierType, Guid> _writeRepository;
    private readonly IdentifierTypeBusinessRules _businessRules;

    public DeleteIdentifierTypeCommandHandler(
        IReadRepository<IdentifierType, Guid> readRepository,
        IWriteRepository<IdentifierType, Guid> writeRepository,
        IdentifierTypeBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteIdentifierTypeCommand request,
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

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

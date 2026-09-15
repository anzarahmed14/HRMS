using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.MaritalStatuses.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Commands.DeleteMaritalStatus;

public class DeleteMaritalStatusCommandHandler
    : IRequestHandler<DeleteMaritalStatusCommand>
{
    private readonly IReadRepository<MaritalStatus, Guid> _readRepository;
    private readonly IWriteRepository<MaritalStatus, Guid> _writeRepository;
    private readonly MaritalStatusBusinessRules _businessRules;

    public DeleteMaritalStatusCommandHandler(
        IReadRepository<MaritalStatus, Guid> readRepository,
        IWriteRepository<MaritalStatus, Guid> writeRepository,
        MaritalStatusBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteMaritalStatusCommand request,
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

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

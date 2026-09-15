using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Genders.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Genders.Commands.DeleteGender;

public class DeleteGenderCommandHandler
    : IRequestHandler<DeleteGenderCommand>
{
    private readonly IReadRepository<Gender, Guid> _readRepository;

    private readonly IWriteRepository<Gender, Guid> _writeRepository;

    private readonly GenderBusinessRules _businessRules;

    public DeleteGenderCommandHandler(
        IReadRepository<Gender, Guid> readRepository,
        IWriteRepository<Gender, Guid> writeRepository,
        GenderBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteGenderCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            await _businessRules.EnsureGenderExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

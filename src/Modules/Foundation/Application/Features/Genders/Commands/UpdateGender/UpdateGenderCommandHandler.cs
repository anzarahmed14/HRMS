using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Genders.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Genders.Commands.UpdateGender;

public class UpdateGenderCommandHandler
    : IRequestHandler<UpdateGenderCommand>
{
    private readonly IReadRepository<Gender, Guid> _readRepository;

    private readonly IWriteRepository<Gender, Guid> _writeRepository;

    private readonly GenderBusinessRules _businessRules;

    public UpdateGenderCommandHandler(
        IReadRepository<Gender, Guid> readRepository,
        IWriteRepository<Gender, Guid> writeRepository,
        GenderBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateGenderCommand request,
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

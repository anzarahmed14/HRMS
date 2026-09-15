using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Languages.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Languages.Commands.UpdateLanguage;

public class UpdateLanguageCommandHandler
    : IRequestHandler<UpdateLanguageCommand>
{
    private readonly IReadRepository<Language, Guid> _readRepository;
    private readonly IWriteRepository<Language, Guid> _writeRepository;
    private readonly LanguageBusinessRules _businessRules;

    public UpdateLanguageCommandHandler(
        IReadRepository<Language, Guid> readRepository,
        IWriteRepository<Language, Guid> writeRepository,
        LanguageBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateLanguageCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            await _businessRules.EnsureLanguageExistsAsync(
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
        entity.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}

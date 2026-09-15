using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Languages.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Languages.Commands.DeleteLanguage;

public class DeleteLanguageCommandHandler
    : IRequestHandler<DeleteLanguageCommand>
{
    private readonly IReadRepository<Language, Guid> _readRepository;
    private readonly IWriteRepository<Language, Guid> _writeRepository;
    private readonly LanguageBusinessRules _businessRules;

    public DeleteLanguageCommandHandler(
        IReadRepository<Language, Guid> readRepository,
        IWriteRepository<Language, Guid> writeRepository,
        LanguageBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteLanguageCommand request,
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

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

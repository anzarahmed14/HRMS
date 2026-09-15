using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Languages.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Languages.Commands.CreateLanguage;

public class CreateLanguageCommandHandler
    : IRequestHandler<CreateLanguageCommand, Guid>
{
    private readonly IWriteRepository<Language, Guid> _writeRepository;
    private readonly LanguageBusinessRules _businessRules;

    public CreateLanguageCommandHandler(
        IWriteRepository<Language, Guid> writeRepository,
        LanguageBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateLanguageCommand request,
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

        var entity = new Language
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
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

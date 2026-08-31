using HRMS.Application.Features.Languages.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Languages.Commands.UpdateEmployeeLanguage;

public class UpdateEmployeeLanguageCommandHandler
    : IRequestHandler<UpdateEmployeeLanguageCommand>
{
    private readonly IReadRepository<EmployeeLanguage, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeLanguage, Guid> _writeRepository;
    private readonly EmployeeLanguageBusinessRules _businessRules;

    public UpdateEmployeeLanguageCommandHandler(
        IReadRepository<EmployeeLanguage, Guid> readRepository,
        IWriteRepository<EmployeeLanguage, Guid> writeRepository,
        EmployeeLanguageBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateEmployeeLanguageCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
        {
            throw new InvalidOperationException(
                "Employee language could not be loaded.");
        }

        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureLanguageExistsAsync(
            request.LanguageId,
            cancellationToken);

        await _businessRules.EnsureLanguageAvailableAsync(
            request.EmployeeId,
            request.LanguageId,
            request.Id,
            cancellationToken);

        entity.EmployeeId = request.EmployeeId;
        entity.LanguageId = request.LanguageId;
        entity.ProficiencyLevel = request.ProficiencyLevel;
        entity.CanRead = request.CanRead;
        entity.CanWrite = request.CanWrite;
        entity.CanSpeak = request.CanSpeak;
        entity.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}

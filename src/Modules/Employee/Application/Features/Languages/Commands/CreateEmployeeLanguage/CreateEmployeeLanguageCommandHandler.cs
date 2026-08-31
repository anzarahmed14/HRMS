using HRMS.Application.Features.Languages.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Languages.Commands.CreateEmployeeLanguage;

public class CreateEmployeeLanguageCommandHandler
    : IRequestHandler<CreateEmployeeLanguageCommand, Guid>
{
    private readonly IWriteRepository<EmployeeLanguage, Guid> _writeRepository;
    private readonly EmployeeLanguageBusinessRules _businessRules;

    public CreateEmployeeLanguageCommandHandler(
        IWriteRepository<EmployeeLanguage, Guid> writeRepository,
        EmployeeLanguageBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmployeeLanguageCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureLanguageExistsAsync(
            request.LanguageId,
            cancellationToken);

        await _businessRules.EnsureLanguageAvailableAsync(
            request.EmployeeId,
            request.LanguageId,
            cancellationToken);

        var entity = new EmployeeLanguage
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            LanguageId = request.LanguageId,
            ProficiencyLevel = request.ProficiencyLevel,
            CanRead = request.CanRead,
            CanWrite = request.CanWrite,
            CanSpeak = request.CanSpeak,
            IsActive = true
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}

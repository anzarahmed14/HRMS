using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Languages.Commands.DeleteEmployeeLanguage;

public class DeleteEmployeeLanguageCommandHandler
    : IRequestHandler<DeleteEmployeeLanguageCommand>
{
    private readonly IReadRepository<EmployeeLanguage, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeLanguage, Guid> _writeRepository;

    public DeleteEmployeeLanguageCommandHandler(
        IReadRepository<EmployeeLanguage, Guid> readRepository,
        IWriteRepository<EmployeeLanguage, Guid> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        DeleteEmployeeLanguageCommand request,
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

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

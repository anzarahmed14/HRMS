using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Experiences.Commands.DeleteEmployeeExperience;

public class DeleteEmployeeExperienceCommandHandler
    : IRequestHandler<DeleteEmployeeExperienceCommand>
{
    private readonly IReadRepository<EmployeeExperience, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeExperience, Guid> _writeRepository;

    public DeleteEmployeeExperienceCommandHandler(
        IReadRepository<EmployeeExperience, Guid> readRepository,
        IWriteRepository<EmployeeExperience, Guid> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        DeleteEmployeeExperienceCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
        {
            throw new InvalidOperationException(
                "Employee experience could not be loaded.");
        }

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

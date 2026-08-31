using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Skills.Commands.DeleteEmployeeSkill;

public class DeleteEmployeeSkillCommandHandler
    : IRequestHandler<DeleteEmployeeSkillCommand>
{
    private readonly IReadRepository<EmployeeSkill, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeSkill, Guid> _writeRepository;

    public DeleteEmployeeSkillCommandHandler(
        IReadRepository<EmployeeSkill, Guid> readRepository,
        IWriteRepository<EmployeeSkill, Guid> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        DeleteEmployeeSkillCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
            throw new InvalidOperationException(
                "Employee skill could not be loaded.");

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

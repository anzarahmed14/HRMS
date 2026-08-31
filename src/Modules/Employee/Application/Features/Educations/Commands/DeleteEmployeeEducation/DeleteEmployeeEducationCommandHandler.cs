using HRMS.Application.Features.Educations.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Educations.Commands.DeleteEmployeeEducation;

public class DeleteEmployeeEducationCommandHandler
    : IRequestHandler<DeleteEmployeeEducationCommand>
{
    private readonly IReadRepository<EmployeeEducation, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeEducation, Guid> _writeRepository;
    private readonly EmployeeEducationBusinessRules _businessRules;

    public DeleteEmployeeEducationCommandHandler(
        IReadRepository<EmployeeEducation, Guid> readRepository,
        IWriteRepository<EmployeeEducation, Guid> writeRepository,
        EmployeeEducationBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteEmployeeEducationCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
        {
            throw new InvalidOperationException(
                "Employee education could not be loaded.");
        }

        await _businessRules.EnsureEmployeeExistsAsync(
            entity.EmployeeId,
            cancellationToken);

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

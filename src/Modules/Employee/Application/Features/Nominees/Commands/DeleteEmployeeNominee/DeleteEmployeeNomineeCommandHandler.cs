using HRMS.Application.Features.Nominees.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Nominees.Commands.DeleteEmployeeNominee;

public class DeleteEmployeeNomineeCommandHandler
    : IRequestHandler<DeleteEmployeeNomineeCommand>
{
    private readonly IReadRepository<EmployeeNominee, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeNominee, Guid> _writeRepository;
    private readonly EmployeeNomineeBusinessRules _businessRules;

    public DeleteEmployeeNomineeCommandHandler(
        IReadRepository<EmployeeNominee, Guid> readRepository,
        IWriteRepository<EmployeeNominee, Guid> writeRepository,
        EmployeeNomineeBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteEmployeeNomineeCommand request,
        CancellationToken cancellationToken)
    {
        var nominee = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (nominee is null)
        {
            throw new InvalidOperationException(
                "Employee nominee could not be loaded.");
        }

        await _businessRules.EnsureEmployeeExistsAsync(
            nominee.EmployeeId,
            cancellationToken);

        await _writeRepository.DeleteAsync(
            nominee,
            cancellationToken);
    }
}

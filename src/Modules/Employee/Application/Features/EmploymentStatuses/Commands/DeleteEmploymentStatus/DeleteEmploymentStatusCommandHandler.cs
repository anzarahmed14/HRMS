using HRMS.Application.Features.EmploymentStatuses.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmploymentStatuses.Commands.DeleteEmploymentStatus;

public class DeleteEmploymentStatusCommandHandler
    : IRequestHandler<DeleteEmploymentStatusCommand>
{
    private readonly IReadRepository<EmploymentStatus, Guid> _readRepository;
    private readonly IWriteRepository<EmploymentStatus, Guid> _writeRepository;
    private readonly EmploymentStatusBusinessRules _businessRules;

    public DeleteEmploymentStatusCommandHandler(
        IReadRepository<EmploymentStatus, Guid> readRepository,
        IWriteRepository<EmploymentStatus, Guid> writeRepository,
        EmploymentStatusBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteEmploymentStatusCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmploymentStatusExistsAsync(
            request.Id,
            cancellationToken);

        var employmentStatus = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employmentStatus is null)
        {
            throw new InvalidOperationException(
                "Employment status could not be loaded.");
        }

        await _writeRepository.DeleteAsync(
            employmentStatus,
            cancellationToken);
    }
}

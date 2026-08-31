using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Certifications.Commands.DeleteEmployeeCertification;

public class DeleteEmployeeCertificationCommandHandler
    : IRequestHandler<DeleteEmployeeCertificationCommand>
{
    private readonly IReadRepository<EmployeeCertification, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeCertification, Guid> _writeRepository;

    public DeleteEmployeeCertificationCommandHandler(
        IReadRepository<EmployeeCertification, Guid> readRepository,
        IWriteRepository<EmployeeCertification, Guid> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        DeleteEmployeeCertificationCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
            throw new InvalidOperationException(
                "Employee certification could not be loaded.");

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Certifications.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Certifications.Commands.DeleteCertification;

public class DeleteCertificationCommandHandler
    : IRequestHandler<DeleteCertificationCommand>
{
    private readonly IReadRepository<Certification, Guid> _readRepository;
    private readonly IWriteRepository<Certification, Guid> _writeRepository;
    private readonly CertificationBusinessRules _businessRules;

    public DeleteCertificationCommandHandler(
        IReadRepository<Certification, Guid> readRepository,
        IWriteRepository<Certification, Guid> writeRepository,
        CertificationBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteCertificationCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            await _businessRules.EnsureCertificationExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}

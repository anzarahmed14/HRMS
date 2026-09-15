using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Certifications.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Certifications.Commands.UpdateCertification;

public class UpdateCertificationCommandHandler
    : IRequestHandler<UpdateCertificationCommand>
{
    private readonly IReadRepository<Certification, Guid> _readRepository;
    private readonly IWriteRepository<Certification, Guid> _writeRepository;
    private readonly CertificationBusinessRules _businessRules;

    public UpdateCertificationCommandHandler(
        IReadRepository<Certification, Guid> readRepository,
        IWriteRepository<Certification, Guid> writeRepository,
        CertificationBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateCertificationCommand request,
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

        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            request.Id,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            name,
            request.Id,
            cancellationToken);

        entity.Code = code;
        entity.Name = name;
        entity.IssuingOrganization = string.IsNullOrWhiteSpace(request.IssuingOrganization)
            ? null
            : request.IssuingOrganization.Trim();
        entity.Description = string.IsNullOrWhiteSpace(request.Description)
            ? null
            : request.Description.Trim();
        entity.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}

using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Certifications.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Certifications.Commands.CreateCertification;

public class CreateCertificationCommandHandler
    : IRequestHandler<CreateCertificationCommand, Guid>
{
    private readonly IWriteRepository<Certification, Guid> _writeRepository;
    private readonly CertificationBusinessRules _businessRules;

    public CreateCertificationCommandHandler(
        IWriteRepository<Certification, Guid> writeRepository,
        CertificationBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateCertificationCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            name,
            cancellationToken);

        var entity = new Certification
        {
            Code = code,
            Name = name,
            IssuingOrganization = string.IsNullOrWhiteSpace(request.IssuingOrganization)
                ? null
                : request.IssuingOrganization.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim(),
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}

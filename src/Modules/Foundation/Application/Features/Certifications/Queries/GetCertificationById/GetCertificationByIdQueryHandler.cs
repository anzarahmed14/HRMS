using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Application.Features.Certifications.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Certifications.Queries.GetCertificationById;

public class GetCertificationByIdQueryHandler
    : IRequestHandler<GetCertificationByIdQuery, CertificationDto>
{
    private readonly IReadRepository<Certification, Guid> _repository;

    public GetCertificationByIdQueryHandler(
        IReadRepository<Certification, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<CertificationDto> Handle(
        GetCertificationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "Certification",
                request.Id);
        }

        return new CertificationDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            IssuingOrganization = entity.IssuingOrganization,
            Description = entity.Description,
            IsActive = entity.IsActive
        };
    }
}

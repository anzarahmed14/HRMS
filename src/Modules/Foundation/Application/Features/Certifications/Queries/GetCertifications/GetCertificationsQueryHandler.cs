using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Certifications.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Certifications.Queries.GetCertifications;

public class GetCertificationsQueryHandler
    : IRequestHandler<
        GetCertificationsQuery,
        PagedResult<CertificationDto>>
{
    private readonly IReadRepository<Certification, Guid> _repository;

    public GetCertificationsQueryHandler(
        IReadRepository<Certification, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<CertificationDto>> Handle(
        GetCertificationsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<CertificationDto>
        {
            Items = result.Items
                .Select(x => new CertificationDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    IssuingOrganization = x.IssuingOrganization,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}

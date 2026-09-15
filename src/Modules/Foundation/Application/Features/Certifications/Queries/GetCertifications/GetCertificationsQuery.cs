using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Certifications.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Certifications.Queries.GetCertifications;

public record GetCertificationsQuery(
    PagedRequest Request) : IRequest<PagedResult<CertificationDto>>;

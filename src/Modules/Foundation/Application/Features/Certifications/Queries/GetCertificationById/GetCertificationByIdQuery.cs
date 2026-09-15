using HRMS.Modules.Foundation.Application.Features.Certifications.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Certifications.Queries.GetCertificationById;

public record GetCertificationByIdQuery(
    Guid Id) : IRequest<CertificationDto>;

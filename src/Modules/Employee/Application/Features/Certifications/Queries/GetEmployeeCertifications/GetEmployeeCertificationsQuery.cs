using HRMS.Application.Features.Certifications.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.Certifications.Queries.GetEmployeeCertifications;

public sealed record GetEmployeeCertificationsQuery(
    Guid EmployeeId,
    PagedRequest Request
) : IRequest<PagedResult<EmployeeCertificationDto>>;

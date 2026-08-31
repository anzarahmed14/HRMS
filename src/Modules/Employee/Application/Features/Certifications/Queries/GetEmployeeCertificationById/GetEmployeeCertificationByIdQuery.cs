using HRMS.Application.Features.Certifications.DTOs;
using MediatR;

namespace HRMS.Application.Features.Certifications.Queries.GetEmployeeCertificationById;

public record GetEmployeeCertificationByIdQuery(Guid Id)
    : IRequest<EmployeeCertificationDto?>;

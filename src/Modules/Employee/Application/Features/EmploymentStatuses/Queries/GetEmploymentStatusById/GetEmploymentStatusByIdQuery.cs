using HRMS.Application.Features.EmploymentStatuses.DTOs;
using MediatR;

namespace HRMS.Application.Features.EmploymentStatuses.Queries.GetEmploymentStatusById;

public record GetEmploymentStatusByIdQuery(Guid Id)
    : IRequest<EmploymentStatusDto?>;

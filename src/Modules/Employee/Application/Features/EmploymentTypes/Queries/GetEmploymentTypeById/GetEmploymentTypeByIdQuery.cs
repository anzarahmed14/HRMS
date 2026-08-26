using HRMS.Application.Features.EmploymentTypes.DTOs;
using MediatR;

namespace HRMS.Application.Features.EmploymentTypes.Queries.GetEmploymentTypeById;

public record GetEmploymentTypeByIdQuery(Guid Id)
    : IRequest<EmploymentTypeDto?>;

using HRMS.Application.Features.Departments.DTOs;
using MediatR;

namespace HRMS.Application.Features.Departments.Queries.GetDepartmentById;

public record GetDepartmentByIdQuery(Guid Id)
    : IRequest<DepartmentDto?>;